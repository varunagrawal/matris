using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Tasks;
using MatrisSVICS.ViewModels;
using MatrisSVICS.Models;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Phone.Net.NetworkInformation;
using System.Diagnostics;

namespace MatrisSVICS.Views
{
    public partial class LocationView : PhoneApplicationPage
    {
        private static List<Place> Places = null;
        private MapRoute mapRoute = null;
        //private GeoCoordinateWatcher watcher;
        private Geoposition geoposition = null;
        private double _accuracy = 0.0;

        public LocationView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent") || (!(bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"]))
            {
                MessageBoxResult result =
                    MessageBox.Show("This app accesses your phone's location.",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No Network Connectivity." + Environment.NewLine + "Please check if you are connected to the Internet.");

                try
                {
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }

            MatrisMap.Visibility = Visibility.Collapsed;
            
            GetPlacesOfInterest();

            //if (watcher == null)
            //{
            //    watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            //    watcher.MovementThreshold = 100;

            //    watcher.PositionChanged += watcher_PositionChanged;

            //    watcher.Start();
            //}
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GetPlacesOfInterest();
        }

        public async void GetPlacesOfInterest()
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                // The user has opted out of Location.
                ApplicationBar.IsVisible = true;
                MapProgressBar.Visibility = Visibility.Collapsed;
                MapProgressBar.IsIndeterminate = false;

                NavigationService.GoBack();

                return;
            }

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            // Show the progress bar and hide the map and Application bar
            MapProgressBar.IsIndeterminate = true;
            MapProgressBar.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = false;

            txtLoadNotif.Visibility = Visibility.Visible;

            try
            {
                // Get Current location
                geoposition = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromSeconds(10), timeout: TimeSpan.FromSeconds(10));

                // Setup the map
                MatrisMap.Center = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                MatrisMap.ZoomLevel = 16;

                txtLoadNotif.Text = "Marking locations!";

                // Get nearby hospitals and Clinics
                Places = await LocationViewModel.GetNearbyPlaces(geoposition.Coordinate.Latitude.ToString("0.000"), geoposition.Coordinate.Longitude.ToString("0.000"));

                Place current = GetCurrentLocation();
                Places.Add(current);

                MapLayer mapLayer = new MapLayer();
                
                _accuracy = geoposition.Coordinate.Accuracy;

                // Draw the location markers on the map
                DrawMapMarkers(mapLayer, Places);

                // Add the overlay layer with the markings.
                MatrisMap.Layers.Add(mapLayer);

                // Show the map
                if (MatrisMap.Visibility != Visibility.Visible)
                {
                    MatrisMap.Visibility = Visibility.Visible;
                }

                ApplicationBar.IsVisible = true;
                MapProgressBar.Visibility = Visibility.Collapsed;
                MapProgressBar.IsIndeterminate = false;

                txtLoadNotif.Visibility = Visibility.Collapsed;
                txtLoadNotif.Text = "Retrieving locations...";
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("Location is disabled in phone settings.");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }

                NavigationService.GoBack();
            }
        }

        /// <summary>
        /// Gets the Place object representing the User's current location.
        /// </summary>
        /// <returns>Place Object that represents the user's current location.</returns>
        private Place GetCurrentLocation()
        {
            Place current = new Place
            {
                Name = "Your location",
                Address = "Present location",
                Geo = new GeoLocation()
            };
            current.Geo.Coordinates = new Location();
            current.Geo.Coordinates.Latitude = geoposition.Coordinate.Latitude;
            current.Geo.Coordinates.Longitude = geoposition.Coordinate.Longitude;

            return current;
        }

        /// <summary>
        /// Draw Pushpins on the Map representing Locations of Interest.
        /// </summary>
        /// <param name="mapLayer">The map layer on which the pushpins are rendered.</param>
        /// <param name="Places">List of Place objects.</param>
        private void DrawMapMarkers(MapLayer mapLayer, List<Place> Places)
        {
            MatrisMap.Layers.Clear();

            foreach (Place p in Places)
            {
                //DrawAccuracyRadius(mapLayer, new GeoCoordinate(p.Geo.Coordinates.Latitude, p.Geo.Coordinates.Longitude));
                
                if (p.Name == "Your location")
                {
                    DrawMapMarker(p, Colors.Green, mapLayer); 
                }
                else
                {
                    DrawMapMarker(p, Colors.Red, mapLayer);
                }
            }
            
        }

        private void DrawMapMarker(Place p, Color color, MapLayer mapLayer)
        {
            // Create a map marker
            Polygon polygon = new Polygon();
            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(-5, 0));
            polygon.Points.Add(new Point(-5, 20));
            polygon.Points.Add(new Point(-25, 20));
            polygon.Points.Add(new Point(-25, 30));
            polygon.Points.Add(new Point(-5, 30));
            polygon.Points.Add(new Point(-5, 50));
            polygon.Points.Add(new Point(5, 50));
            polygon.Points.Add(new Point(5, 30));
            polygon.Points.Add(new Point(25, 30));
            polygon.Points.Add(new Point(25, 20));
            polygon.Points.Add(new Point(5, 20));
            polygon.Points.Add(new Point(5, 0));
            
            polygon.Fill = new SolidColorBrush(color);

            // Enable marker to be tapped for location information
            polygon.Tag = p;
            polygon.MouseLeftButtonUp += new MouseButtonEventHandler(Marker_Click);

            // Create a MapOverlay and add marker
            MapOverlay overlay = new MapOverlay();
            overlay.Content = polygon;
            overlay.GeoCoordinate = new GeoCoordinate(p.Geo.Coordinates.Latitude, p.Geo.Coordinates.Longitude);
            overlay.PositionOrigin = new Point(0.0, 1.0);
            mapLayer.Add(overlay);
        }

        private void Marker_Click(object sender, EventArgs e)
        {
            Polygon p = (Polygon)sender;
            Place place = (Place)p.Tag;

            CustomMessageBox cmb = new CustomMessageBox()
            {
                Caption = "Hospital/Clinic Details",
                Message = string.Format("Name: {0}\n\nAddress: {1}", place.Name, place.Address),
                LeftButtonContent = "Get Route",
                RightButtonContent = "Dismiss"
            };

            cmb.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            GetRoute(GetCurrentLocation(), place);
                            break;
                        case CustomMessageBoxResult.RightButton:
                            break;
                        case CustomMessageBoxResult.None:
                            break;
                        default:
                            break;
                    }
                };

            cmb.Show();
            
        }

        /// <summary>
        /// Mark route on Map from "from" place to "to" place.
        /// </summary>
        /// <param name="from">Source location</param>
        /// <param name="to">Destination location</param>
        private void GetRoute(Place from, Place to)
        {
            List<GeoCoordinate> routeCoordinates = new List<GeoCoordinate>();
            routeCoordinates.Add(new GeoCoordinate(from.Geo.Coordinates.Latitude, from.Geo.Coordinates.Longitude));
            routeCoordinates.Add(new GeoCoordinate(to.Geo.Coordinates.Latitude, to.Geo.Coordinates.Longitude));

            RouteQuery routeQuery = new RouteQuery();
            routeQuery.TravelMode = TravelMode.Driving;
            routeQuery.Waypoints = routeCoordinates;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            routeQuery.QueryAsync();
        }

        private void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                if (mapRoute != null)
                {
                    MatrisMap.RemoveRoute(mapRoute);
                }
                
                Route route = e.Result;
                mapRoute = new MapRoute(route);

                MatrisMap.AddRoute(mapRoute);
            }
        }

        private void DrawAccuracyRadius(MapLayer mapLayer, GeoCoordinate geocoord)
        {
            // The ground resolution (in meters per pixel) varies depending on the level of detail
            // and the latitude at which it’s measured. It can be calculated as follows:
            double metersPerPixels = (Math.Cos(geocoord.Latitude * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, MatrisMap.ZoomLevel));
            double radius = _accuracy / metersPerPixels;

            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius;
            ellipse.Height = radius;
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(75, 200, 0, 0));

            MapOverlay overlay = new MapOverlay();
            overlay.Content = ellipse;
            overlay.GeoCoordinate = new GeoCoordinate(geocoord.Latitude, geocoord.Longitude);
            overlay.PositionOrigin = new Point(0.5, 0.5);
            mapLayer.Add(overlay);  
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            GetPlacesOfInterest();
        }

        private void LaunchBingMaps_Click(object sender, EventArgs e)
        {
            LaunchMapsTask();
        }

        private void LaunchMapsTask()
        {
            BingMapsTask bingMapsTask = new BingMapsTask();

            bingMapsTask.SearchTerm = "hospital";
            bingMapsTask.ZoomLevel = 2;

            bingMapsTask.Show();
        }
    }
}