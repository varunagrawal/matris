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
using System.Collections.ObjectModel;

namespace MatrisSVICS.Views
{
    public partial class GalleryView : PhoneApplicationPage
    {
        private ObservableCollection<Pic> _pics = new ObservableCollection<Pic>();
        public IEnumerable<Pic> Pics
        {
            get { return _pics; }
        }

        public GalleryView()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // We are coming back from Images Page
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                CameraImagesListBox.ItemsSource = Pics;
                CameraImagesListBox.SelectedIndex = -1;
                return;
            }

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

            if (cameraRoll != null)
            {
                foreach (Picture p in cameraRoll.Pictures)
                {
                    BitmapImage b = new BitmapImage();
                    b.SetSource(p.GetThumbnail());
                    _pics.Add(new Pic() { ImageUrl = b, Name = p.Name });
                }
            }
            
        }

        private void CameraImagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If real selection is happened, go to a ImagesPage
            if (CameraImagesListBox.SelectedIndex == -1) return;
            this.NavigationService.Navigate(new Uri("/Views/ImagesView.xaml?SelectedIndex=" + CameraImagesListBox.SelectedIndex, UriKind.Relative));
        }
    }

    
    public class Pic
    {
        public BitmapImage ImageUrl { get; set; }
        public string Name { get; set; }
    }
}