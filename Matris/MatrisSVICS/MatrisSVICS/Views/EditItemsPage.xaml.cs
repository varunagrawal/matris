using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MatrisSVICS.ViewModels;

namespace MatrisSVICS.Views
{
    public partial class EditItemsPage : PhoneApplicationPage
    {
        public EditItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            DataContext = new EditItemsViewModel(PhoneApplicationService.Current.State["CurrentModel"]);
        }
    }
}