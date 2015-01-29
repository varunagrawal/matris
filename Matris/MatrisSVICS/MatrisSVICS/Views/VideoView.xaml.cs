using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.ComponentModel;
using System.Threading;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Devices;
using System.Windows.Media;

namespace MatrisSVICS.Views
{
    public partial class VideoView : PhoneApplicationPage
    {
        // Viewfinder for capturing video.
        private VideoBrush videoRecorderBrush;

        // Source and device for capturing video.
        private CaptureSource captureSource;
        private VideoCaptureDevice videoCaptureDevice;

        // File details for storing the recording.        
        private IsolatedStorageFileStream isoVideoFile;
        private FileSink fileSink;
        private string isoVideoFileName = "Matris_Recording.mp4";

        // For managing button and application state.
        private enum ButtonState { Initialized, Ready, Recording, Playback, Paused, NoChange, CameraNotSupported };
        private ButtonState currentAppState;

        public VideoView()
        {
            InitializeComponent();

            // Prepare ApplicationBar and buttons.
            PhoneAppBar = (ApplicationBar)ApplicationBar;
            PhoneAppBar.IsVisible = true;
            Playback = ((ApplicationBarIconButton)ApplicationBar.Buttons[0]);
            Stop = ((ApplicationBarIconButton)ApplicationBar.Buttons[1]);
            Record = ((ApplicationBarIconButton)ApplicationBar.Buttons[2]);
            Picture = ((ApplicationBarIconButton)ApplicationBar.Buttons[3]);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationService.RemoveBackEntry();

            // Initialize the video recorder.
            InitializeVideoRecorder();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Dispose of camera and media objects.
            DisposeVideoPlayer();
            DisposeVideoRecorder();

            base.OnNavigatedFrom(e);
        }

        // Update the buttons and text on the UI thread based on app state.
        private void UpdateUI(ButtonState currentButtonState, string statusMessage)
        {
            // Run code on the UI thread.
            Dispatcher.BeginInvoke(delegate
            {

                switch (currentButtonState)
                {
                    // When the camera is not supported by the device.
                    case ButtonState.CameraNotSupported:
                        Record.IsEnabled = false;
                        Stop.IsEnabled = false;
                        Playback.IsEnabled = false;
                        break;

                    // First launch of the application, so no video is available.
                    case ButtonState.Initialized:
                        Record.IsEnabled = true;
                        Stop.IsEnabled = false;
                        Playback.IsEnabled = false;
                        break;

                    // Ready to record, so video is available for viewing.
                    case ButtonState.Ready:
                        Record.IsEnabled = true;
                        Stop.IsEnabled = false;
                        Playback.IsEnabled = true;
                        break;

                    // Video recording is in progress.
                    case ButtonState.Recording:
                        Record.IsEnabled = false;
                        Stop.IsEnabled = true;
                        Playback.IsEnabled = false;
                        break;

                    // Video playback is in progress.
                    case ButtonState.Playback:
                        Record.IsEnabled = false;
                        Stop.IsEnabled = true;
                        Playback.IsEnabled = false;
                        break;

                    default:
                        break;
                }

                // Display a message.
                txtDebug.Text = statusMessage;

                // Note the current application state.
                currentAppState = currentButtonState;
            });
        }

        public void InitializeVideoRecorder()
        {
            if (captureSource == null)
            {
                // Create the VideoRecorder objects.
                captureSource = new CaptureSource();
                fileSink = new FileSink();

                videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

                // Add eventhandlers for captureSource.
                captureSource.CaptureFailed += new EventHandler<ExceptionRoutedEventArgs>(OnCaptureFailed);

                // Initialize the camera if it exists on the device.
                if (videoCaptureDevice != null)
                {
                    // Create the VideoBrush for the viewfinder.
                    videoRecorderBrush = new VideoBrush();
                    videoRecorderBrush.SetSource(captureSource);

                    // Display the viewfinder image on the rectangle.
                    viewfinderRectangle.Fill = videoRecorderBrush;

                    // Start video capture and display it on the viewfinder.
                    captureSource.Start();

                    // Set the button state and the message.
                    UpdateUI(ButtonState.Initialized, "Tap record to start recording...");
                }
                else
                {
                    // Disable buttons when the camera is not supported by the device.
                    UpdateUI(ButtonState.CameraNotSupported, "A camera is not supported on this device.");
                }
            }
        }

        // Set recording state: start recording.
        private void StartVideoRecording()
        {
            try
            {
                // Connect fileSink to captureSource.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Connect the input and output of fileSink.
                    fileSink.CaptureSource = captureSource;
                    fileSink.IsolatedStorageFileName = isoVideoFileName;
                }

                // Begin recording.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Stopped)
                {
                    captureSource.Start();
                }

                // Set the button states and the message.
                UpdateUI(ButtonState.Recording, "Recording...");
            }

            // If recording fails, display an error.
            catch (Exception e)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    txtDebug.Text = "ERROR: " + e.Message.ToString();
                });
            }
        }

        // Set the recording state: stop recording.
        private void StopVideoRecording()
        {
            try
            {
                // Stop recording.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Disconnect fileSink.
                    fileSink.CaptureSource = null;
                    fileSink.IsolatedStorageFileName = null;

                    // Set the button states and the message.
                    UpdateUI(ButtonState.NoChange, "Preparing viewfinder...");

                    StartVideoPreview();
                }
            }
            // If stop fails, display an error.
            catch (Exception e)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    txtDebug.Text = "ERROR: " + e.Message.ToString();
                });
            }
        }

        // Set the recording state: display the video on the viewfinder.
        private void StartVideoPreview()
        {
            try
            {
                // Display the video on the viewfinder.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Stopped)
                {
                    // Add captureSource to videoBrush.
                    videoRecorderBrush.SetSource(captureSource);

                    // Add videoBrush to the visual tree.
                    viewfinderRectangle.Fill = videoRecorderBrush;

                    captureSource.Start();

                    // Set the button states and the message.
                    UpdateUI(ButtonState.Ready, "Ready to record.");
                }
            }
            // If preview fails, display an error.
            catch (Exception e)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    txtDebug.Text = "ERROR: " + e.Message.ToString();
                });
            }
        }

        // Start the video recording.
        private void Record_Click(object sender, EventArgs e)
        {
            // Avoid duplicate taps.
            Record.IsEnabled = false;

            StartVideoRecording();
        }

        // Handle stop requests.
        private void Stop_Click(object sender, EventArgs e)
        {
            // Avoid duplicate taps.
            Stop.IsEnabled = false;

            // Stop during video recording.
            if (currentAppState == ButtonState.Recording)
            {
                StopVideoRecording();

                // Set the button state and the message.
                UpdateUI(ButtonState.NoChange, "Recording stopped.");
            }

            // Stop during video playback.
            else
            {
                // Remove playback objects.
                DisposeVideoPlayer();

                StartVideoPreview();

                // Set the button state and the message.
                UpdateUI(ButtonState.NoChange, "Playback stopped.");
            }
        }

        // Start video playback.
        private void Playback_Click(object sender, EventArgs e)
        {
            // Avoid duplicate taps.
            Playback.IsEnabled = false;

            // Start video playback when the file stream exists.
            if (isoVideoFile != null)
            {
                VideoPlayer.Play();
            }
            // Start the video for the first time.
            else
            {
                // Stop the capture source.
                captureSource.Stop();

                // Remove VideoBrush from the tree.
                viewfinderRectangle.Fill = null;

                // Create the file stream and attach it to the MediaElement.
                isoVideoFile = new IsolatedStorageFileStream(isoVideoFileName,
                                        FileMode.Open, FileAccess.Read,
                                        IsolatedStorageFile.GetUserStoreForApplication());

                VideoPlayer.SetSource(isoVideoFile);

                // Add an event handler for the end of playback.
                VideoPlayer.MediaEnded += new RoutedEventHandler(VideoPlayerMediaEnded);

                // Start video playback.
                VideoPlayer.Play();
            }

            // Set the button state and the message.
            UpdateUI(ButtonState.Playback, "Playback started.");
        }


        private void DisposeVideoPlayer()
        {
            if (VideoPlayer != null)
            {
                // Stop the VideoPlayer MediaElement.
                VideoPlayer.Stop();

                // Remove playback objects.
                VideoPlayer.Source = null;
                isoVideoFile = null;

                // Remove the event handler.
                VideoPlayer.MediaEnded -= VideoPlayerMediaEnded;
            }
        }

        private void DisposeVideoRecorder()
        {
            if (captureSource != null)
            {
                // Stop captureSource if it is running.
                if (captureSource.VideoCaptureDevice != null
                    && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();
                }

                // Remove the event handlers for capturesource and the shutter button.
                captureSource.CaptureFailed -= OnCaptureFailed;

                // Remove the video recording objects.
                captureSource = null;
                videoCaptureDevice = null;
                fileSink = null;
                videoRecorderBrush = null;
            }
        }

        // If recording fails, display an error message.
        private void OnCaptureFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                txtDebug.Text = "ERROR: " + e.ErrorException.Message.ToString();
            });
        }

        // Display the viewfinder when playback ends.
        public void VideoPlayerMediaEnded(object sender, RoutedEventArgs e)
        {
            // Remove the playback objects.
            DisposeVideoPlayer();

            StartVideoPreview();
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/CameraView.xaml?video=true", UriKind.RelativeOrAbsolute));
        }

    }
}