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
    public partial class AddEventView : PhoneApplicationPage
    {
        #region Members
        private EventModel addEventView = new EventModel();
        #endregion Members

        #region Constructor
        public AddEventView()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Methods
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void ReturnToMainPage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MatrisCalendarMainView.xaml", UriKind.Relative));
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

            if (eventNameNew.Text == string.Empty)
            {
                MessageBox.Show("Please enter a valid name for the event e.g. Visit Doctor for 3rd check up");
                return;
            }
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

            if ( Convert.ToInt32(ageOfChildMonthNew.Text) > 12 || Convert.ToInt32(ageOfChildMonthNew.Text) < 0)
            {
                MessageBox.Show("Please enter a valid value for months of yours child age e.g. 2 for February");
                return;
            }
            if (!( Convert.ToInt32(ageOfChildDayNew.Text) <= 30 && Convert.ToInt32(ageOfChildDayNew.Text) > 0 ))
            {
                MessageBox.Show("Please enter a valid value for days of yours child age e.g. 12");
                return;
            }

            addEventView.EventName = eventNameNew.Text;
            addEventView.EventDescription = eventDescriptionNew.Text;
            DateTime childDateOfBirth = (DateTime)IsolatedStorageSettings.ApplicationSettings["ChildDateOfBirth"];
            
            //Check if user is providing age of child in past
            if ((childDateOfBirth.AddYears(newAgeOfChildYear).AddMonths(newAgeOfChildMonth).AddDays(newAgeOfChildDay)).CompareTo(DateTime.Today) <= 0)
            {
                MessageBox.Show("Child's age for the event should be in future");
                return;
            }
            else 
            {
                EventListDatabaseContext dbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
                addEventView.AgeOfChildYear = newAgeOfChildYear;
                addEventView.AgeOfChildMonth = newAgeOfChildMonth;
                addEventView.AgeOfChildDay = newAgeOfChildDay;
                addEventView.IsAllottedAppointment = false;
                dbContext.Events.InsertOnSubmit(addEventView);
                dbContext.SubmitChanges();
                dbContext.Dispose();
            }
            //edit reminder for the event
            MatrisReminder matrisReminder = new MatrisReminder();
            matrisReminder.EditReminder(addEventView);
            
            NavigationService.GoBack();
        }

        #endregion Methods
    }
}