using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    public class Installation
    {
        public string id { get; set; }

        [JsonProperty(PropertyName="phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        public Installation(string phoneNumber, string userName)
        {
            this.PhoneNumber = phoneNumber;
            this.UserName = userName;
        }
    }
}
