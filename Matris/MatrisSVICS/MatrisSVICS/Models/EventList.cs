using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    class EventList
    {
        #region Properties
        [JsonConstructor]
        public EventList() { }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "eventname")]
        public string EventName { get; set; }

        //Description of the event
        [JsonProperty(PropertyName= "eventdescription")]
        public string EventDescription {get; set;}

        //Age of child for the event
        [JsonProperty(PropertyName="ageofchildyear")]
        public int AgeOfChildYear {get; set;}

        [JsonProperty(PropertyName="ageofchildmonth")]
        public int AgeOfChildMonth { get; set; }

        [JsonProperty(PropertyName="ageofchildday")]
        public int AgeOfChildDay { get; set; }

        //Whether the appointment has been set or not
        [JsonProperty(PropertyName="isallottedappointment")]
        public bool IsAllottedAppointment { get; set; }


        #endregion Properties
    }
}
