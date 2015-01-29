using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MatrisSVICS.Views
{
    public partial class HealthPageView : PhoneApplicationPage
    {
        public HealthPageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string PageLink = "";
            NavigationContext.QueryString.TryGetValue("selectedItem", out PageLink);

            HealthPageBrowser.Navigate(new Uri(PageLink, UriKind.RelativeOrAbsolute));

        }
    }
}