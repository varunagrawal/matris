using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MatrisSVICS.Resources;
using MatrisSVICS.ViewModels;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Phone.Notification;
using System.Text;
using System.IO.IsolatedStorage;

namespace MatrisSVICS
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            HubTileService.FreezeGroup("matris");
            //VisualStateManager.GoToState(hubTile_Itemlist, "Flipped", true);
            VisualStateManager.GoToState(hubTile_Calendar, "Flipped", true);
            VisualStateManager.GoToState(hubTile_Location, "Flipped", true);
            //VisualStateManager.GoToState(hubTile_Camera, "Flipped", true);
            VisualStateManager.GoToState(hubTile_Health, "Flipped", true);
        }

        private void hubTile_Location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LocationView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void hubTile_Itemlist_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LoggedInOnce"))
            {
                if (Convert.ToString(IsolatedStorageSettings.ApplicationSettings["LoggedInOnce"]).ToLower() == "true")
                {
                    App.RootFrame.Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
                }

                else
                {
                    App.RootFrame.Navigate(new Uri("/Views/RegistrationPage.xaml", UriKind.Relative));
                }
            }

            else
            {
                App.RootFrame.Navigate(new Uri("/Views/RegistrationPage.xaml", UriKind.Relative));
            }

        }

        private void hubTile_Calendar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!(IsolatedStorageSettings.ApplicationSettings.Contains("ChildDateOfBirth")))
            {
                //Ask for child's DOB
                //Ask for Childbirth date
                NavigationService.Navigate(new Uri("/Views/ChildDateOfBirthView.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
            NavigationService.Navigate(new Uri("/Views/MatrisCalendarMainView.xaml", UriKind.RelativeOrAbsolute));
        }

        }

        private void hubTile_Camera_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/CameraView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void hubTile_Health_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/HealthView.xaml", UriKind.RelativeOrAbsolute));
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ReadMe.xaml", UriKind.RelativeOrAbsolute));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

    }
}