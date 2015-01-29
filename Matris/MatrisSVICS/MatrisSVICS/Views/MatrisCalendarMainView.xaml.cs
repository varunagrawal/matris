using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MatrisSVICS.Resources;
using MatrisSVICS.Calendar.ViewModel;
using MatrisSVICS.Calendar.Model;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using MatrisSVICS;
using Microsoft.WindowsAzure.MobileServices;
using MatrisSVICS.Models;
using System.Diagnostics;

namespace MatrisSVICS.Calendar
{
    public partial class MatrisCalendarMainView : PhoneApplicationPage
    {
        #region Members
        private EventViewModel eventViewModel = new EventViewModel();
        #endregion Members

        #region Constructor
        public MatrisCalendarMainView()
        {
            InitializeComponent();
            eventViewModel = new EventViewModel();
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        #endregion Constructor

        #region Methods

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool firstLaunchFlag = (bool)IsolatedStorageSettings.ApplicationSettings["FirstLaunchFlag"];

            try
            {
                string fromDOBPage = "false";
                NavigationContext.QueryString.TryGetValue("DOB", out fromDOBPage);

                if (fromDOBPage == "true")
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }           

            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
            SystemTray.ProgressIndicator.Text = "Loading...";

            //First time launch of app
            if (firstLaunchFlag == true)
            {
                // Check for Network Connectivity. If not available, then show message and exit.
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())// NetworkInterface.GetIsNetworkAvailable())
                {
                    MessageBox.Show("No Network Connectivity." + Environment.NewLine + "Please check if you are connected to the Internet.");

                    if (NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                }

                //Get the Data from SQL Azure and store to local DB
                IMobileServiceTable<EventList> eventTable = App.MobileService.GetTable<EventList>();
                IEnumerable<EventList> eventListTemp;
                IMobileServiceTableQuery<EventList> query = eventTable.Where(t => t.IsAllottedAppointment == false);
                try
                {
                    eventListTemp = await eventTable.ReadAsync<EventList>(query);
                    ObservableCollection<EventList> eventList = new ObservableCollection<EventList>(eventListTemp);
                    EventModel eventModel;
                    for (int index = 0; index < eventListTemp.Count(); index++)
                    {
                        eventModel = new EventModel(eventList[index].EventName, eventList[index].EventDescription, eventList[index].AgeOfChildYear, eventList[index].AgeOfChildMonth, eventList[index].AgeOfChildDay, eventList[index].IsAllottedAppointment);
                        eventViewModel.EventList.Add(eventModel);
                    }
                    eventViewModel.SaveNewEventList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Network issues, please check your internet connectivity and try again!");
                    Debug.WriteLine(ex.Message);

                    if(NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }

                    return;
                }


                //Reminder Setting
                MatrisReminder matrisReminder = new MatrisReminder();

                foreach (EventModel eventModel in eventViewModel.EventList)
                {
                    //Set reminder
                    matrisReminder.AddReminder(eventModel);
                }
                //Set the value of the FirstLaunchFlag to false
                IsolatedStorageSettings.ApplicationSettings["FirstLaunchFlag"] = false;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            else if (firstLaunchFlag == false)
            {
                eventViewModel.GetSavedEventList();
            }

            SystemTray.ProgressIndicator.IsIndeterminate = false;
            SystemTray.ProgressIndicator.IsVisible = false;

            EventListPanel.DataContext = eventViewModel.EventList;       

        }

        /// <summary>
        ///Add a new event like vaccination, doctor's visit etc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEvent(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AddEventView.xaml", UriKind.Relative));
        }


        private void EventSelected(object sender, SelectionChangedEventArgs e)
        {
            if (EventListPanel.SelectedItem != null)
            {
                PhoneApplicationService.Current.State["selectedEventDetails"] = (EventModel)EventListPanel.SelectedItem;
                NavigationService.Navigate(new Uri("/Views/EventDetailsView.xaml", UriKind.Relative));
            }
        }

        #endregion Methods


    }
}