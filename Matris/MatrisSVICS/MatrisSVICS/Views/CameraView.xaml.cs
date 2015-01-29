using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Devices;
using System.Windows.Media;
using System.Diagnostics;

namespace MatrisSVICS.Views
{
    public partial class CameraView : PhoneApplicationPage
    {
        private int savedCounter = 0;
        PhotoCamera cam;
        MediaLibrary library = new MediaLibrary();

        public CameraView()
        {
            InitializeComponent();
        }

        //Code for initialization, capture completed, image availability events; also setting the source for the viewfinder.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string fromVideoRecorder = "false";
                NavigationContext.QueryString.TryGetValue("video", out fromVideoRecorder);

                if (fromVideoRecorder == "true")
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            // Check to see if the camera is available on the phone.
            if ((PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true) ||
                 (PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) == true))
            {
                // Initialize the camera, when available.
                cam = new PhotoCamera(CameraType.Primary);

                // Event is fired when the PhotoCamera object has been initialized.
                cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(cam_Initialized);

                // Event is fired when the capture sequence is complete.
                cam.CaptureCompleted += new EventHandler<CameraOperationCompletedEventArgs>(cam_CaptureCompleted);

                // Event is fired when the capture sequence is complete and an image is available.
                cam.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(cam_CaptureImageAvailable);

                // Event is fired when the capture sequence is complete and a thumbnail image is available.
                cam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(cam_CaptureThumbnailAvailable);

                // The event is fired when the shutter button receives a half press.
                CameraButtons.ShutterKeyHalfPressed += OnButtonHalfPress;

                // The event is fired when the shutter button receives a full press.
                CameraButtons.ShutterKeyPressed += OnButtonFullPress;

                // The event is fired when the shutter button is released.
                CameraButtons.ShutterKeyReleased += OnButtonRelease;

                //Set the VideoBrush source to the camera.
                viewfinderBrush.SetSource(cam);

                if (IsolatedStorageSettings.ApplicationSettings.Contains("ImageCounter"))
                {
                    savedCounter = (int )IsolatedStorageSettings.ApplicationSettings["ImageCounter"];
                }

            }
            else
            {
                // The camera is not supported on the phone.
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Write message.
                    txtInfo.Text = "A Camera is not available on this phone.";
                });

                // Disable UI.
                PhotoClick.IsEnabled = false;
            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (cam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                cam.Dispose();

                // Release memory, ensure garbage collection.
                cam.Initialized -= cam_Initialized;
                cam.CaptureCompleted -= cam_CaptureCompleted;
                cam.CaptureImageAvailable -= cam_CaptureImageAvailable;
                cam.CaptureThumbnailAvailable -= cam_CaptureThumbnailAvailable;

                CameraButtons.ShutterKeyHalfPressed -= OnButtonHalfPress;
                CameraButtons.ShutterKeyPressed -= OnButtonFullPress;
                CameraButtons.ShutterKeyReleased -= OnButtonRelease;

                if(!IsolatedStorageSettings.ApplicationSettings.Contains("ImageCounter"))
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("ImageCounter", savedCounter);
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["ImageCounter"] = savedCounter;
                }
            }
        }

        // Update the UI if initialization succeeds.
        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Write message.
                    txtInfo.Text = "Camera initialized.";
                });
            }
        }

        // Ensure that the viewfinder is upright in LandscapeRight.
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if (cam != null)
            {
                // LandscapeRight rotation when camera is on back of phone.
                int landscapeRightRotation = 180;

                // Change LandscapeRight rotation for front-facing camera.
                if (cam.CameraType == CameraType.FrontFacing) landscapeRightRotation = -180;

                // Rotate video brush from camera.
                if (e.Orientation == PageOrientation.LandscapeRight)
                {
                    // Rotate for LandscapeRight orientation.
                    viewfinderBrush.RelativeTransform =
                        new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = landscapeRightRotation };
                }
                else
                {
                    // Rotate for standard landscape orientation.
                    viewfinderBrush.RelativeTransform =
                        new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
                }
            }

            base.OnOrientationChanged(e);
        }

        private void PhotoClick_Click(object sender, EventArgs e)
        {
            if (cam != null)
            {
                try
                {
                    // Start image capture.
                    cam.CaptureImage();
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        // Cannot capture an image until the previous capture has completed.
                        txtInfo.Text = ex.Message;
                    });
                }
            }
        }

        void cam_CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            // Increments the savedCounter variable used for generating JPEG file names.
            savedCounter++;
        }


        // Informs when full resolution photo has been taken, saves to local media library and the local folder.
        void cam_CaptureImageAvailable(object sender, Microsoft.Devices.ContentReadyEventArgs e)
        {
            string fileName = "Matris_" + savedCounter + ".jpg";

            try
            {   // Write message to the UI thread.
                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    txtInfo.Text = "Captured image available, saving photo.";
                });

                // Save photo to the media library camera roll.
                library.SavePictureToCameraRoll(fileName, e.ImageStream);

                // Write message to the UI thread.
                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    txtInfo.Text = "Photo has been saved to camera roll.";

                });

                // Set the position of the stream back to start
                e.ImageStream.Seek(0, SeekOrigin.Begin);
            }
            finally
            {
                // Close image stream
                e.ImageStream.Close();
            }

        }

        // Informs when thumbnail photo has been taken, saves to the local folder
        // User will select this image in the Photos Hub to bring up the full-resolution. 
        public void cam_CaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            try
            {
                // Write message to UI thread.
                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    txtInfo.Text = "Captured image available, saving thumbnail.";
                });

            }
            finally
            {
                // Close image stream
                e.ImageStream.Close();
            }
        }

        // Provide auto-focus with a half button press using the hardware shutter button.
        private void OnButtonHalfPress(object sender, EventArgs e)
        {
            if (cam != null)
            {
                // Focus when a capture is not in progress.
                try
                {
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        txtInfo.Text = "Half Button Press: Auto Focus";
                    });

                    cam.Focus();
                }
                catch (Exception focusError)
                {
                    // Cannot focus when a capture is in progress.
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        txtInfo.Text = focusError.Message;
                    });
                }
            }
        }

        // Capture the image with a full button press using the hardware shutter button.
        private void OnButtonFullPress(object sender, EventArgs e)
        {
            if (cam != null)
            {
                cam.CaptureImage();

                txtInfo.Text = "Image captured";
            }
        }

        // Cancel the focus if the half button press is released using the hardware shutter button.
        private void OnButtonRelease(object sender, EventArgs e)
        {
            if (cam != null)
            {
                try
                {
                    //cam.CancelFocus();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    //cam.CancelFocus();
                }
            }
        }

        private void Video_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/VideoView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Gallery_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/GalleryView.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}