using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

namespace MatrisSVICS.ViewModels
{
    class ItemList
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "sender")]
        public string SenderId { get; set; }

        [JsonProperty(PropertyName = "receiver")]
        public string ReceiverId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        public ItemList(string _channel, string _sender, string _receiver, string _name, int _quantity)
        {
            Channel = _channel;
            SenderId = _sender;
            ReceiverId = _receiver;
            ItemName = _name;
            Quantity = _quantity;
        }
    }

    class ItemListViewModel
    {
        public static async void AddItem(string sender, string receiver, string item, string quantity)
        {
            IMobileServiceTable<ItemList> notificationsTable = App.MobileService.GetTable<ItemList>();

            ItemList newItem = new ItemList(App.pushChannel.ChannelUri.ToString(), sender, receiver, item, int.Parse(quantity));
            await notificationsTable.InsertAsync(newItem);
        }
    }
}
