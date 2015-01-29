using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace MatrisSVICS.Views
{
    public partial class ChildDateOfBirthView : PhoneApplicationPage
    {
        public ChildDateOfBirthView()
        {
            InitializeComponent();
        }

        private void SaveChildDateOfBirth(object sender, EventArgs e)
        {
            DateTime childDateOfBirth;

            try
            {
                childDateOfBirth = (DateTime)childDateOfBirthPicker.Value;
            }
            catch(Exception)
            {
                MessageBox.Show("Please enter the correct date of birth e.g for 26th February 2012 enter 26 in Days, 2 in Months and 2012 in Year");
                return;
            }

            if (!(IsolatedStorageSettings.ApplicationSettings.Contains("ChildDateOfBirth")))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("ChildDateOfBirth", childDateOfBirth);
            }

            NavigationService.Navigate(new Uri("/Views/MatrisCalendarMainView.xaml?DOB=true", UriKind.RelativeOrAbsolute));
        }
    }
}