﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    public class Registrations
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "handle")]
        public string Handle { get; set; }
    }
}