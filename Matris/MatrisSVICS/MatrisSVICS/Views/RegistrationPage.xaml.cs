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
    public partial class RegistrationPage : PhoneApplicationPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);

            DataContext = new InstallationViewModel();
        }
    }
}