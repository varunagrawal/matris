using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MatrisSVICS.Calendar.ViewModel;
using MatrisSVICS.Calendar.Model;
using System.Windows.Media;
using MatrisSVICS;
using System.IO.IsolatedStorage;

namespace MatrisSVICS.Calendar
{
    public partial class EditEventView : PhoneApplicationPage
    {
        #region Members
        private EventViewModel editEventViewModel = null;
        private EventModel editEventView = null;
        #endregion Members

        #region Constructor
        public EditEventView()
        {
            InitializeComponent();
            editEventViewModel = new EventViewModel();
        }
        #endregion Constructor

        #region Methods
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Get the data from Model through View-Model
            editEventView = (EventModel)PhoneApplicationService.Current.State["selectedEventDetails"];

            eventName.DataContext = editEventView.EventName;
            eventDescriptionNew.DataContext = editEventView.EventDescription;
            ageOfChildYearsNew.DataContext = editEventView.AgeOfChildYear;
            ageOfChildMonthNew.DataContext = editEventView.AgeOfChildMonth;
            ageOfChildDayNew.DataContext = editEventView.AgeOfChildDay;
            if (editEventView.IsAllottedAppointment == true)
            { appointmentStatus.Fill = new SolidColorBrush(Colors.Orange); }
            else if (editEventView.IsAllottedAppointment == false)
            { appointmentStatus.Fill = new SolidColorBrush(Colors.Red); }
        }

        private void ReturnToMainPage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/EventDetailsView.xaml", UriKind.Relative));
        }
        /// <summary>
        /// Edit the existing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEvent(object sender, EventArgs e)
        {
            int newAgeOfChildYear;
            int newAgeOfChildMonth;
            int newAgeOfChildDay;

            try
            {
                newAgeOfChildYear = Convert.ToInt32(ageOfChildYearsNew.Text);
                newAgeOfChildMonth = Convert.ToInt32(ageOfChildMonthNew.Text);
                newAgeOfChildDay = Convert.ToInt32(ageOfChildDayNew.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter numeric values for year, month and days");
                return;
            }

            if ((Convert.ToInt32(ageOfChildMonthNew.Text) > 12) || (Convert.ToInt32(ageOfChildMonthNew.Text) < 0))
            {
                MessageBox.Show("Please enter a valid value for months of yours child age e.g.2 for February");
                return;
            }
            if (!((Convert.ToInt32(ageOfChildDayNew.Text) <= 30) && (Convert.ToInt32(ageOfChildDayNew.Text) > 0)))
            {
                MessageBox.Show("Please enter a valid value for days of yours child age e.g.12");
                return;
            }
            string newEventDescription = eventDescriptionNew.Text.ToString();
            editEventView.EventDescription = newEventDescription;
            DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];
            
            //Check if user is providing age of child in past
            if ((childDateOfBirth.AddYears(newAgeOfChildYear).AddMonths(newAgeOfChildMonth).AddDays(newAgeOfChildDay)).CompareTo(DateTime.Today) <= 0)
            {
                MessageBox.Show("Child's age for the event should be in future");
                return;
            }
            else if ((childDateOfBirth.AddYears(newAgeOfChildYear).AddMonths(newAgeOfChildMonth).AddDays(newAgeOfChildDay)).CompareTo(DateTime.Today) > 0)
            {
                EventListDatabaseContext dbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
                EventModel existingEvent = (from EventModel eventItem in dbContext.Events where eventItem.EventId == editEventView.EventId select eventItem).SingleOrDefault();
                existingEvent.EventDescription = newEventDescription;
                existingEvent.AgeOfChildYear = newAgeOfChildYear;
                existingEvent.AgeOfChildMonth = newAgeOfChildMonth;
                existingEvent.AgeOfChildDay = newAgeOfChildDay;
                existingEvent.IsAllottedAppointment = false;
                dbContext.SubmitChanges();
                dbContext.Dispose();
            }  
     
            else
            {
                EventListDatabaseContext dbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
                EventModel existingEvent = (from EventModel eventItem in dbContext.Events where eventItem.EventId == editEventView.EventId select eventItem).SingleOrDefault();
                existingEvent.EventDescription = newEventDescription;
                existingEvent.AgeOfChildYear = newAgeOfChildYear;
                existingEvent.AgeOfChildMonth = newAgeOfChildMonth;
                dbContext.SubmitChanges();
                dbContext.Dispose();
            }

            //edit reminder for the event
            MatrisReminder matrisReminder = new MatrisReminder();
            matrisReminder.EditReminder(editEventView);

            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        #endregion Methods
    }
}