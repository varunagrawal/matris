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

namespace MatrisSVICS.Views
{
    public partial class Health : PhoneApplicationPage
    {
        private static HealthViewModel viewModel = new HealthViewModel();
        
        private static string Category = "";

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static HealthViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
        }


        public Health()
        {
            InitializeComponent();

            if (!ViewModel.IsDataLoaded)
            {
                ViewModel.LoadData();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("selectedItem", out Category);

            PageTitle.Text = Category.ToLowerInvariant();

            if (!string.IsNullOrEmpty(Category))
            {
                ViewModel.LoadData();
                ViewModel.Items = new ObservableCollection<HealthModel>(ViewModel.Items.Where(x => x.Category == Category).ToList<HealthModel>());
            }
            DataContext = ViewModel;

        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Views/HealthPageView.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as HealthModel).PageLink, UriKind.RelativeOrAbsolute));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }
    }
}