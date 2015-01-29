using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    public class CompositeNotification : INotifyPropertyChanged
    {
        public string IconUri { get; set; }

        public Notifications NotificationItem { get; set; }

        public bool IsMarkedDone { get; set; }

        public CompositeNotification(Notifications item, string oldOrNew)
        {
            if (oldOrNew.Equals("old"))
            {
                this.IconUri = "/Assets/Icons/feature.email.png";
            }

            else
            {
                this.IconUri = "/Assets/Icons/New.png";
            }

            this.NotificationItem = item;
            this.IsMarkedDone = false;
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
