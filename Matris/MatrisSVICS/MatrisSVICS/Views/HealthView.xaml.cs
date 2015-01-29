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
using MatrisSVICS.Models;
using System.Collections.ObjectModel;
using Microsoft.Phone.Net.NetworkInformation;
using System.Diagnostics;

namespace MatrisSVICS.Views
{
    public partial class HealthCategoryView : PhoneApplicationPage
    {
        public HealthCategoryView()
        {
            InitializeComponent();

            HealthCategoryViewModel categoryVM = new HealthCategoryViewModel();
            categoryVM.Categories = GetCategories();
            DataContext = categoryVM;
        }

        public ObservableCollection<string> GetCategories()
        {
            return new ObservableCollection<string>(HealthModel.GetPagesList().Select(x => x.Category).Distinct<string>().ToList<string>());
            // categories;
        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            //MessageBox.Show((MainLongListSelector.SelectedItem as string));
            NavigationService.Navigate(new Uri("/Views/HealthCategoryView.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as string), UriKind.RelativeOrAbsolute));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check for Network Connectivity. If not available, then show message and exit.
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())// NetworkInterface.GetIsNetworkAvailable())
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
        }
    }
}