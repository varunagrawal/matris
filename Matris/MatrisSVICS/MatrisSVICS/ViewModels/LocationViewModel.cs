using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatrisSVICS.Models;
using System.Net.Http;
using System.IO;
using System.Xml.Serialization;

namespace MatrisSVICS.ViewModels
{
    public class LocationViewModel
    {
        public static string API_KEY = "AIzaSyAA92VBXjbRxiNFwKNRt29jDj95qQcJKtA";
        
        public static async Task<List<Place>> GetNearbyPlaces(string latitude, string longitude)
        {
            PlaceSearch Results;
            try
            {
                string PlacesXML = await GetPlaces(latitude, longitude);   
            
                using (TextReader reader = new StringReader(PlacesXML))
                {
                    XmlSerializer deser = new XmlSerializer(typeof(PlaceSearch));

                    Results = (PlaceSearch)deser.Deserialize(reader);
                }

                return Results.Places;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private static async Task<string> GetPlaces(string latitude, string longitude)
        {
            try
            {
                //string BaseUrl = "https://maps.googleapis.com/maps/api/place/textsearch/xml?";
                string BaseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/xml?";
                
                List<KeyValuePair<string, string>> Values = new List<KeyValuePair<string, string>>
                {
                    //new KeyValuePair<string, string>("query", "hospital"),
                    new KeyValuePair<string, string>("key", API_KEY),
                    new KeyValuePair<string, string>("sensor", "true"),
                    new KeyValuePair<string, string>("location", latitude.ToString() + "," + longitude.ToString()),
                    new KeyValuePair<string, string>("radius", "5000"),
                    new KeyValuePair<string, string>("types", "hospital"),
                    new KeyValuePair<string, string>("name", "hospital")
                };

                string[] parameters = new string[6];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = string.Join("=", new string[] { Values[i].Key, Values[i].Value });
                }

                Uri Places_Url = new Uri(BaseUrl + string.Join("&", parameters));

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(Places_Url);
                response.EnsureSuccessStatusCode();

                string xml = await response.Content.ReadAsStringAsync();

                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
