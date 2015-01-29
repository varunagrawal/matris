using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MatrisSVICS.Models
{
    public class Notifications : INotifyPropertyChanged
    {
        public string ID { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "itemname")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "optionalmessage")]
        public string OptionalMessage { get; set; }

        [JsonProperty(PropertyName = "channelname")]
        public string ChannelName { get; set; }

        public Notifications()
        {

        }

        public Notifications(string fromName, string toID, Items fromItem, int quantity, string message)
        {
            From = fromName;
            To = toID;
            this.ItemName = fromItem.ItemName;
            this.Quantity = quantity;
            this.OptionalMessage = message;
            this.ChannelName = "Chaitra-Shalini";
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
