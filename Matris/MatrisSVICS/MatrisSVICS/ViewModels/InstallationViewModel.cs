using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MatrisSVICS.Models;
using Microsoft.Phone.Info;
using Microsoft.WindowsAzure.MobileServices;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Collections;
using Microsoft.Phone.UserData;
using System.ComponentModel;
using MatrisSVICS.ViewModels;
using System.Windows.Navigation;


namespace MatrisSVICS.ViewModels
{
    public class InstallationViewModel : INotifyPropertyChanged
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public Task<Int32> result;
        //contacts related

        public List<CustomContact> contact { get; set; }
        public List<string> phonenums;
        public MobileServiceCollection<Installation, Installation> items;

        public IMobileServiceTable<Installation> todoTable = App.MobileService.GetTable<Installation>();
        public CustomContact customcont;
        public ContactsViewModel conVM;

        //added
        private Visibility _isPageVisible;


        public Visibility IsPageVisible
        {
            get
            {
                return _isPageVisible;
            }
            set
            {
                if (_isPageVisible == value) return;
                _isPageVisible = value;
                NotifyPropertyChanged("IsPageVisible");
            }
        }

        private ICommand _saveInstallationCommand;

        //  [IgnoreDataMember]
        public ICommand SaveInstallationCommand
        {
            get
            {
                return _saveInstallationCommand;
            }
            set
            {
                _saveInstallationCommand = value;
            }
        }


        public InstallationViewModel()
        {
            //contatcs

            //  conVM = new ContactsViewModel();
            //  result = conVM.RefreshTodoItems();
            IsPageVisible = Visibility.Visible;
            //customcont = new CustomContact();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("PhoneNumber"))
                PhoneNumber = (string)IsolatedStorageSettings.ApplicationSettings["PhoneNumber"];
            if (IsolatedStorageSettings.ApplicationSettings.Contains("UserName"))
                UserName = (string)IsolatedStorageSettings.ApplicationSettings["UserName"];
            _saveInstallationCommand = new DelegateCommand(ClickNext);
            //  dummy();

        }

        private void ClickNext(object p)
        {
            IsPageVisible = Visibility.Visible;
            //Save persistent tags to settings, don't show page again
            if (PhoneNumber == null || UserName == null)
            {
                MessageBox.Show("Please enter the fields ", "Manditory fields", MessageBoxButton.OK);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["LoggedInOnce"] = true;
                IsolatedStorageSettings.ApplicationSettings["PhoneNumber"] = PhoneNumber;
                IsolatedStorageSettings.ApplicationSettings["UserName"] = UserName;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
                //int flag = result.Result;
                //if (flag == 1)
                //{


                //    //Add code to navigate to Contacts page, request to choose buddy
                //    conVM.contactMethod();
                //}
                SaveSettings();

            }



            //this is shifted to contactsDisplayPage
            //Now set up the push channel
            //  Toast.AcquirePushChannel();

            //Save this user to the cloud DB


            //Save isolated settings



            //this is shifted to contatcsdisplay page.
            //Navigate to items page
            //    PhoneApplicationService.Current.State["PreviousModel"] = this;
            //   (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative)); 
        }

        private void SaveSettings()
        {
            Object _sync = new Object();
            lock (_sync)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public async static Task<string> GetUniqueServerID(string phoneNumber)
        {
            IMobileServiceTable<Installation> installationTable = App.MobileService.GetTable<Installation>();

            IMobileServiceTableQuery<string> query =
                installationTable.
                Where(installationItem => installationItem.PhoneNumber == phoneNumber).Select(installationItem => installationItem.id);
            try
            {
                IEnumerable<string> resultSet = await installationTable.ReadAsync<string>(query);
                return resultSet.FirstOrDefault<string>();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return "";

        }


        ////conttacts

        //void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        //{

        //    try
        //    {
        //        //Bind the results to the user interface.
        //        //ContactResultsData.DataContext = e.Results;
        //        contact = new List<CustomContact>();
        //        foreach (var con in e.Results)
        //        {
        //            var num = con.PhoneNumbers.FirstOrDefault();
        //            customcont.Number = num.ToString();
        //            customcont.Name = con.DisplayName;
        //        }
        //        CompareWitCloudData(customcont, phonenums);
        //        //  IsolatedStorageSettings.ApplicationSettings["BuddyID"]
        //        PhoneApplicationService.Current.State["contactsModel"] = this;
        //     //   IsolatedStorageSettings.ApplicationSettings["contactsList"] = contact;
        //        (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/Contacts.xaml", UriKind.Relative));
        //     //   string st = "shalini";

        //    }
        //    catch (System.Exception)
        //    {
        //        //No results
        //    }
        //}
        //public void CompareWitCloudData(CustomContact con, List<string> phonenums)
        //{
        //    //    var result = Regex.Match(con.Number.ToLower(), @"\d+").Value;

        //    for (int k = 0; k < phonenums.Count; k++)
        //    {

        //        var getNumbers = (from t in con.Number.ToLower()
        //                          where char.IsDigit(t)
        //                          select t).ToArray();
        //        string st = new string(getNumbers);
        //        //  string st = (con.Number.ToLower()).Substring(0, (con.Number.ToLower()).IndexOf('(') - 1);
        //        if (st == (phonenums[k].ToLower()))
        //        {
        //            contact.Add(con);
        //        }
        //    }
        //}
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public class Installation
        //{
        //    public string Id { get; set; }

        //    [JsonProperty(PropertyName = "username")]
        //    public string UserName { get; set; }

        //    [JsonProperty(PropertyName = "phonenumber")]
        //    public string PhoneNumber { get; set; }

        //}
    }
}
