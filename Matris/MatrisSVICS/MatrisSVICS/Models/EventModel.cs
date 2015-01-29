using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.Serialization;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;


namespace MatrisSVICS.Calendar.Model
{
    [Table]
    public class EventModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region PrivateMembers
        private int _eventId ;
        private string _eventName = null;
        private string _eventDescription = null;
        private int _ageOfChildYear = 0;
        private int _ageOfChildMonth = 0;
        private int _ageOfChildDay = 0;
        private bool _isAllottedAppointment = false;


        #endregion PrivateMembers

        #region PublicMembers
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;
        #endregion PublicMembers

        #region Constructor
        public EventModel()
        {

        }

        public EventModel(string eventName, string eventDescription, int ageOfChildYear, int ageOfChildMonth, int ageOfChildDay, bool isAllottedAppointment)
        {
            _eventName = eventName;
            _eventDescription = eventDescription;
            _ageOfChildDay = ageOfChildDay;
            _ageOfChildMonth = ageOfChildMonth;
            _ageOfChildYear = ageOfChildYear;
            _isAllottedAppointment = isAllottedAppointment;
        }
        #endregion Constructor

        #region Properties

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int EventId
        {
            get
            {
                return _eventId;
            }
            set
            {
                RaisePropertyChanging("EventId");
                _eventId = value;
                RaisePropertyChanged("EventId");
            }
        }
        //Name of the event: Tetanus Vaccination

        [Column]
        public string EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                RaisePropertyChanging("EventName");
                _eventName = value;
                RaisePropertyChanged("EventName");
            }
        }

        //Description of the event
        [Column]
        public string EventDescription
        {
            get
            {
                return _eventDescription;
            }
            set
            {
                RaisePropertyChanging("EventDescription");
                _eventDescription = value;
                RaisePropertyChanged("EventDescription");
            }
        }

        //Age of child for the event
        [Column]
        public int AgeOfChildYear
        {
            get
            {
                return _ageOfChildYear;
            }
            set
            {
                RaisePropertyChanging("AgeOfChildYear");
                _ageOfChildYear = value;
                RaisePropertyChanged("AgeOfChildYear");
            }
        }
        [Column]
        public int AgeOfChildMonth
        {
            get
            {
                return _ageOfChildMonth;
            }
            set
            {
                RaisePropertyChanging("AgeOfChildMonth");
                _ageOfChildMonth = value;
                RaisePropertyChanged("AgeOfChildMonth");
            }
        }
        [Column]
        public int AgeOfChildDay
        {
            get
            {
                return _ageOfChildDay;
            }
            set
            {
                RaisePropertyChanging("AgeOfChildDay");
                _ageOfChildDay = value;
                RaisePropertyChanged("AgeOfChildDay");
            }
        }

        //Whether the appointment has been set or not
        [Column]
        public bool IsAllottedAppointment
        {
            get
            {
                return _isAllottedAppointment;
            }
            set
            {
                RaisePropertyChanging("IsAllottedAppointment");
                _isAllottedAppointment = value;
                RaisePropertyChanged("IsAllottedAppointment");
            }
        }



        #endregion Properties

        #region Methods

        //Method to be called when any of the data model values change and view needs to be updated
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RaisePropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion Methods
    }

}
