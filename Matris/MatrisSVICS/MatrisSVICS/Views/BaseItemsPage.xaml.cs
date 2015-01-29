using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using MatrisSVICS.ViewModels;
//using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;

namespace MatrisSVICS.Views
{
    public partial class BaseItemsPage : PhoneApplicationPage
    {
        public BaseItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //NavigationService.RemoveBackEntry();

            //await AuthenticateAsync();
            base.OnNavigatedTo(e);
            DataContext = new BaseItemViewModel();
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //if (e.NavigationMode == NavigationMode.Back)
            //    NavigationService.Navigate(new Uri("/Views/ToDOListMain.xaml", UriKind.Relative));
        }

        //private MobileServiceUser user;
        //private async System.Threading.Tasks.Task AuthenticateAsync()
        //{
        //    while (user == null)
        //    {
        //        string message;
        //        try
        //        {
        //            user = await App.MobileService
        //                .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
        //            message =
        //                string.Format("You are now logged in - {0}", user.UserId);
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            message = "You must log in. Login Required";
        //        }

        //        var dialog = new Dialog(message);
        //        dialog.Commands.Add(new UICommand("OK"));
        //        await dialog.ShowAsync();
        //    }
        //}
    }
}