using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;

namespace MatrisSVICS.Views
{
    public partial class ImagesView : PhoneApplicationPage
    {
        int selectedImageIndex = -1;

        public ImagesView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Find selected image index from parameters
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("SelectedIndex"))
            {
                selectedImageIndex = Int32.Parse(parameters["SelectedIndex"]);
            }
            else
            {
                selectedImageIndex = 0;
            }
            
            // Load image
            LoadImage();
        }

        private void LoadImage()
        {
            // We are coming from MainPage, start loading album images 
            MediaPlayer.Queue.ToString();

            MediaLibrary ml = null;
            PictureAlbum cameraRoll = null;

            foreach (MediaSource source in MediaSource.GetAvailableMediaSources())
            {
                if (source.MediaSourceType == MediaSourceType.LocalDevice)
                {
                    ml = new MediaLibrary(source);
                    PictureAlbumCollection allAlbums = ml.RootPictureAlbum.Albums;

                    cameraRoll = allAlbums.Where(x => x.Name == "Camera Roll").FirstOrDefault<PictureAlbum>();

                }
            }

            BitmapImage b = new BitmapImage();
            b.SetSource(cameraRoll.Pictures[selectedImageIndex].GetImage());
            image.Source = b;
        }
    }
}