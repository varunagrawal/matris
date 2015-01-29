using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MatrisSVICS.Calendar.Model;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using MatrisSVICS.Calendar.ViewModel;
using MatrisSVICS;
using System.IO.IsolatedStorage;

namespace MatrisSVICS.Calendar
{
    public partial class EventDetailsView : PhoneApplicationPage
    {
        public EventModel selectedEvent = null;
        public EventDetailsView()
        {
            InitializeComponent();
            selectedEvent = (EventModel)PhoneApplicationService.Current.State["selectedEventDetails"];

            if (selectedEvent != null)
            {
                eventName.DataContext = selectedEvent.EventName;
                ageOfChildYearsNew.DataContext = selectedEvent.AgeOfChildYear;
                ageOfChildMonthNew.DataContext = selectedEvent.AgeOfChildMonth;
                ageOfChildDayNew.DataContext = selectedEvent.AgeOfChildDay;
                eventDescription.DataContext = selectedEvent.EventDescription;
                //eventDetails.DataContext = selectedEvent.EventSelected;
            }
        }

        /// <summary>
        /// Edit the selected event - redirect to EditEvent page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditEvent(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/EditEventView.xaml", UriKind.Relative));
        }
         /// <summary>
         /// Return to the MATRIS Calendar home page
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void ReturnToHomePage(object sender, EventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        /// <summary>
        /// Edit/Add/Delete Appointment (WP8 calendar)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAppointment(object sender, EventArgs e)
        {
            SaveAppointmentTask saveAppointment = new SaveAppointmentTask();
            DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];

            DateTime _startTime = childDateOfBirth.AddYears(selectedEvent.AgeOfChildYear);
            _startTime = _startTime.AddMonths(selectedEvent.AgeOfChildMonth);
            _startTime = _startTime.AddDays(selectedEvent.AgeOfChildDay);
            //DateTime _startTime = childDateOfBirth.AddDays(selectedEvent.AgeOfChildYear * 365 + selectedEvent.AgeOfChildMonth * 30 + selectedEvent.AgeOfChildDay);
            saveAppointment.StartTime = _startTime;
            saveAppointment.EndTime = _startTime.AddDays(1);
            saveAppointment.Subject = selectedEvent.EventName;
            saveAppointment.Details = selectedEvent.EventDescription;
            saveAppointment.IsAllDayEvent = true;
            saveAppointment.Reminder = Reminder.OneWeek; //.EighteenHours;
            saveAppointment.Location = "<Please enter the venue>";
            saveAppointment.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.OutOfOffice;

            saveAppointment.Show();
            
            //TODO: Check if entry is set from Windows Phone Calendar
            EventListDatabaseContext dbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
            EventModel eventWithAppointmentSet = (from EventModel eventItem in dbContext.Events where eventItem.EventId == selectedEvent.EventId select eventItem).SingleOrDefault();
            eventWithAppointmentSet.IsAllottedAppointment = true;
            dbContext.SubmitChanges();
            dbContext.Dispose();
        }

        private void DeleteEvent(object sender, EventArgs e)
        {
            //Delete reminder
            MatrisReminder matrisReminder = new MatrisReminder();
            matrisReminder.DeleteReminder(selectedEvent);
            
            //Delete from DB
            EventListDatabaseContext dbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
            EventModel eventToBeDeleted = (from EventModel eventItem in dbContext.Events where eventItem.EventId == selectedEvent.EventId select eventItem).SingleOrDefault();
            dbContext.Events.DeleteOnSubmit(eventToBeDeleted);
            dbContext.SubmitChanges();
            dbContext.Dispose();

            NavigationService.GoBack();
            //NavigationService.Navigate(new Uri("/Views/MatrisCalendarMainView.xaml", UriKind.Relative));
        }

        
    }
}