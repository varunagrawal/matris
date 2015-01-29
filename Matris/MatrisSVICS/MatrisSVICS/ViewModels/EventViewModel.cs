using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using MatrisSVICS.Calendar.Model;
using System.Windows.Media;
using Microsoft.WindowsAzure.MobileServices;
using MatrisSVICS;
using System.Windows;

namespace MatrisSVICS.Calendar.ViewModel
{
    public class EventViewModel
    {
        #region PrivateMembers
        static private ObservableCollection<EventModel> eventList = new ObservableCollection<EventModel>();
        static private EventModel selectedEvent = null;
        #endregion PrivateMembers

        #region Properties
        //List of Events
        public ObservableCollection<EventModel> EventList
        {
            get
            {
                return eventList;
            }
            set
            {
                eventList = value;
            }
        }

        public EventModel SelectedEvent
        {
            get
            {
                return selectedEvent;
            }
            set
            {
                selectedEvent = value;
            }
        }
        #endregion Properties

        
        #region Methods
        /// <summary>
        /// Saves the data to local DB
        /// </summary>
        public void SaveNewEventList()
        {
            EventListDatabaseContext eventListDbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
            foreach (var eventItem in eventList)
            {
                eventListDbContext.Events.InsertOnSubmit(eventItem);
            }
            eventListDbContext.SubmitChanges();
            eventListDbContext.Dispose();
        }
        /// <summary>
        /// Populates view with data from Local Database
        /// </summary>
        public void GetSavedEventList()
        {
            EventListDatabaseContext eventListDbContext = new EventListDatabaseContext(EventListDatabaseContext.dbConnectionString);
            var eventListTemp = from EventModel eventItem in eventListDbContext.Events select eventItem;
            eventList = new ObservableCollection<EventModel>(eventListTemp);
            eventListDbContext.Dispose();
        }
        /// <summary>
        /// Populates view with default data (placeholder for testing without connecting to DB)
        /// </summary>
        public void GetNewEventList()
        {
            //IMobileServiceTable<EventList> eventTable = App.MobileService.GetTable<EventList>();
            ////ObservableCollection<EventList> eventListTemp = new ObservableCollection<EventList>();
            ObservableCollection<EventModel> eventListTemp = new ObservableCollection<EventModel>();
            //eventListTemp.Add(new EventList { EventId = 1, EventName = "TestEventName1", EventDescription = "Test Description", AgeOfChildYear = 2, AgeOfChildMonth = 3, AgeOfChildDay = 24, IsAllottedAppointment = false });
            //eventListTemp.Add(new EventList { EventId = 2, EventName = "TestEventName2", EventDescription = "Test Description", AgeOfChildYear = 3, AgeOfChildMonth = 4, AgeOfChildDay = 1, IsAllottedAppointment = false });
            //eventList = eventListTemp;
            //IMobileServiceTableQuery<EventModel> query = eventTable.ToListAsync();
            eventListTemp.Add(new EventModel("Hepatitis B1 (HepB)", "Hepatitis B1 (HepB) Injection to be given at birth", 0, 0, 0, false));
            eventListTemp.Add(new EventModel("Hepatitis B1 (HepB)", "Hepatitis B1 (HepB) Injection to be given between 1st and 2nd month", 0, 1, 0, false));
            eventListTemp.Add(new EventModel("Hepatitis B1 (HepB)", "Hepatitis B1 (HepB) Injection to be given between 6th and 18th month", 0, 6, 0, false));
            eventListTemp.Add(new EventModel("Rotavirus2 (RV) RV1 (2-dose series); RV5 (3-dose series)", "1st Dose to be given at 2 months", 0, 2, 0, false));
            eventListTemp.Add(new EventModel("Rotavirus2 (RV) RV1 (2-dose series); RV5 (3-dose series)", "2nd Dose to be given at 4 months", 0, 4, 0, false));
            eventListTemp.Add(new EventModel("Rotavirus2 (RV) RV1 (2-dose series); RV5 (3-dose series)", "3rd Dose to be given at 6 months", 0, 6, 0, false));
            eventListTemp.Add(new EventModel("Diphtheria, tetanus, & acel- lular pertussis3 (DTaP: <7 yrs", "1st Dose to be given at 2 months", 0, 2, 0, false));
            eventListTemp.Add(new EventModel("Diphtheria, tetanus, & acel- lular pertussis3 (DTaP: <7 yrs", "2nd Dose to be given at 4 months", 0, 4, 0, false));
            eventListTemp.Add(new EventModel("Diphtheria, tetanus, & acel- lular pertussis3 (DTaP: <7 yrs", "3rd Dose to be given at 6 months", 0, 6, 0, false));
            eventListTemp.Add(new EventModel("Diphtheria, tetanus, & acel- lular pertussis3 (DTaP: <7 yrs", "4th Dose to be given between 15 and 18 months", 0, 15, 0, false));
            eventListTemp.Add(new EventModel("Diphtheria, tetanus, & acel- lular pertussis3 (DTaP: <7 yrs", "5th Dose to be given at age fo 4-6 years", 4, 0, 1, false));
            eventListTemp.Add(new EventModel("Haemophilus influenzae type b5 (Hib)", "1st Dose to be given at 2 months", 0, 2, 0, false));
            eventListTemp.Add(new EventModel("Haemophilus influenzae type b5 (Hib)", "2nd Dose to be given at 4 months", 0, 4, 0, false));
            eventListTemp.Add(new EventModel("Haemophilus influenzae type b5 (Hib)", "3rd Dose to be given at 6 months", 0, 6, 0, false));
            eventListTemp.Add(new EventModel("Haemophilus influenzae type b5 (Hib)", "4th Dose to be given between 15 and 18 months", 0, 15, 0, false));
            eventList = eventListTemp;
            SaveNewEventList();
        }

        public void SaveItemToLocalDatabase(EventListDatabaseContext dbContext, EventModel eventItem)
        {
            dbContext.Events.InsertOnSubmit(eventItem);
        }

        #endregion Methods
    }
}

