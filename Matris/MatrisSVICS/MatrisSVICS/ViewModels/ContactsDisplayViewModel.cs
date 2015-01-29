using MatrisSVICS.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatrisSVICS.ViewModels
{
    class ContactsDisplayViewModel
    {
        public List<CustomContact> Contacts { get; set; }
        IsolatedStorageSettings settings;
        private CustomContact _currentlySelectedItem;
        public CustomContact CurrentlySelectedItem
        {
            get { return _currentlySelectedItem; }
            set
            {
                _currentlySelectedItem = value;
                selectBuddy();
            }
        }
        private Visibility _isContactsEnabled;
        public Visibility IsContactsEnabled
        {
            get
            {
                return _isContactsEnabled;
            }

            set
            {
                if (_isContactsEnabled == value) return;
                _isContactsEnabled = value;
                NotifyPropertyChanged("IsContactsEnabled");

            }

        }


        private Visibility _isContactTextVisible;
        public Visibility IsContactTextVisible
        {
            get
            {
                return _isContactTextVisible;
            }

            set
            {
                if (_isContactTextVisible == value) return;
                _isContactTextVisible = value;
                NotifyPropertyChanged("IsContactTextVisible");

            }

        }

        private string _noContactsText;
        public string NoContactsText
        {
            get
            {
                return _noContactsText;
            }

            set
            {
                if (_noContactsText == value) return;
                _noContactsText = value;
                NotifyPropertyChanged("NoContactsText");

            }

        }

        public ContactsDisplayViewModel(object p)
        {
            this.Contacts = ((ContactsViewModel)p).contact;
            if (Contacts.Count == 0)
            {
                IsContactsEnabled = Visibility.Collapsed;
                NoContactsText = "None from your phone contacts have registered the app";
                IsContactTextVisible = Visibility.Visible;
            }
            else
            {
                IsContactTextVisible = Visibility.Collapsed;
                IsContactsEnabled = Visibility.Visible;
            }

        }
        public void selectBuddy()
        {
            IsContactsEnabled = Visibility.Collapsed;


            settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("BuddyValue"))
            {
                //send push notification

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Contact", "Add " + CurrentlySelectedItem.Name.ToLower() + " as Buddy?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    
                    String bdset = "";
                    foreach (var con in Contacts)
                    {
                        if (con.Name.ToLower() == CurrentlySelectedItem.Name.ToLower())
                        {
                            var getNumbers = (from t in con.Number.ToLower()
                                              where char.IsDigit(t)
                                              select t).ToArray();
                            bdset = new string(getNumbers);

                        }
                    }

                    IsolatedStorageSettings.ApplicationSettings["BuddyValue"] = bdset;
                    //   IsolatedStorageSettings.ApplicationSettings["LoggedInOnce"] = true;
                    SaveSettings();
                    AddRegistrationTOCloud();
                }
                //Now set up the push channel
                // Toast.AcquirePushChannel();

                PhoneApplicationService.Current.State["PreviousModel"] = this;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));

                //i dono 
                //   IsContactsEnabled = Visibility.Visible;
            }

        }
        public async void AddRegistrationTOCloud()
        {
            try
            {
                IMobileServiceTable<Installation> installationTable = App.MobileService.GetTable<Installation>();
                string phonenumber = (string)IsolatedStorageSettings.ApplicationSettings["PhoneNumber"];
                string username = (string)IsolatedStorageSettings.ApplicationSettings["UserName"];
                await installationTable.InsertAsync(new Installation(phonenumber, username));
            }
            catch (MobileServiceInvalidOperationException e) 
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
        private void SaveSettings()
        {
            Object _sync = new Object();
            lock (_sync)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
