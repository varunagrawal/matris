using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MatrisSVICS.Models
{
    [XmlRoot("PlaceSearchResponse")]
    public class PlaceSearch
    {
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("result")]
        public List<Place> Places { get; set; }

        [XmlElement("next_page_token")]
        public string NextPageToken { get; set; }
    }

    public class Place
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("type")]
        public List<string> Types { get; set; }

        [XmlElement("vicinity")]
        public string Address { get; set; }

        [XmlElement("geometry")]
        public GeoLocation Geo { get; set; }

        [XmlElement("rating")]
        public string Rating { get; set; }

        [XmlElement("icon")]
        public string Icon { get; set; }

        [XmlElement("photo")]
        public Photo Pic { get; set; }
    }

    public class GeoLocation
    {
        [XmlElement("location")]
        public Location Coordinates { get; set; }
    }

    public class Location
    {
        [XmlElement("lat")]
        public double Latitude { get; set; }

        [XmlElement("lng")]
        public double Longitude { get; set; }
    }

    public class Photo
    {
        [XmlElement("photo_reference")]
        public string Reference { get; set; }

        [XmlElement("width")]
        public string Width { get; set; }

        [XmlElement("height")]
        public string Height { get; set; }
    }
}
