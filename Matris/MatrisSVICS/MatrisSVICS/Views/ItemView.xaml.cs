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
    public partial class ItemView : PhoneApplicationPage
    {
        public ItemView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Added for testing purposes
            ItemListViewModel.AddItem("Varun", "Chaitra", "Sample", "1");
        }
    }
}