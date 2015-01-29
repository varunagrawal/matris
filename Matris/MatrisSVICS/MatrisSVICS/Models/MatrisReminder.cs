using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrisSVICS.Calendar.Model;
using MatrisSVICS;
using System.IO.IsolatedStorage;
using MatrisSVICS.Calendar.ViewModel;
using Microsoft.Phone.Scheduler;
//using Microsoft.Phone.Tasks;

namespace MatrisSVICS.Calendar
{
    class MatrisReminder
    {
        /// <summary>
        /// Adds the reminder for an event
        /// </summary>
        /// <param name="selectedEvent">The entire </param>
        public void AddReminder(EventModel selectedEvent)
        {
            Reminder reminder = ScheduledActionService.Find(selectedEvent.EventId.ToString()) as Reminder;
            if (reminder == null)
            {
                reminder = new Reminder(selectedEvent.EventId.ToString());
                reminder.Title = selectedEvent.EventName;
                reminder.Content = selectedEvent.EventDescription;
                DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];

                DateTime beginTime = GetBeginTime(childDateOfBirth, selectedEvent); 
                reminder.BeginTime = beginTime.AddDays(-8);

                reminder.RecurrenceType = RecurrenceInterval.Weekly;
                ScheduledActionService.Add(reminder);
            }         

        }

        /// <summary>
        /// Gets the reminder associated with the event and edit it with the new date
        /// </summary>
        /// <param name="selectedEvent"></param>
        public void EditReminder(EventModel selectedEvent)
        {
            Reminder reminder = ScheduledActionService.Find(selectedEvent.EventId.ToString()) as Reminder;
            if (reminder == null)
            {
                AddReminder(selectedEvent);
            }
            else
            {
                reminder.Title = selectedEvent.EventName;
                reminder.Content = selectedEvent.EventDescription;
                DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];

                DateTime beginTime = GetBeginTime(childDateOfBirth, selectedEvent);
                reminder.BeginTime = beginTime.AddDays(-8);

                reminder.RecurrenceType = RecurrenceInterval.Weekly;
                ScheduledActionService.Replace(reminder);
            }

        }

        /// <summary>
        /// Deletes the reminder associated with the event 
        /// </summary>
        /// <param name="selectedEvent"></param>
        public void DeleteReminder(EventModel selectedEvent)
        {
            Microsoft.Phone.Scheduler.ScheduledActionService.Remove(selectedEvent.EventId.ToString());
        }

        public void AddToCalendar(EventModel selectedEvent)
        {
            Microsoft.Phone.Tasks.SaveAppointmentTask saveAppointment = new Microsoft.Phone.Tasks.SaveAppointmentTask();

            DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];
            DateTime _startTime = GetBeginTime(childDateOfBirth, selectedEvent);

            saveAppointment.StartTime = _startTime;
            saveAppointment.EndTime = _startTime.AddDays(1);
            saveAppointment.Subject = selectedEvent.EventName;
            saveAppointment.Details = selectedEvent.EventDescription;
            saveAppointment.IsAllDayEvent = true;
            saveAppointment.Reminder = Microsoft.Phone.Tasks.Reminder.OneWeek; //.EighteenHours;
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

        private DateTime GetBeginTime(DateTime ChildDateOfBirth, EventModel SelectedEvent)
        {
            DateTime BeginTime = ChildDateOfBirth.AddYears(SelectedEvent.AgeOfChildYear);
            BeginTime = BeginTime.AddMonths(SelectedEvent.AgeOfChildMonth);
            BeginTime = BeginTime.AddDays(SelectedEvent.AgeOfChildDay);

            return BeginTime;
        }

    }
}
