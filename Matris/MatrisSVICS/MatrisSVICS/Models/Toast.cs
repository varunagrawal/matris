using Microsoft.Phone.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MatrisSVICS.ViewModels;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace MatrisSVICS.Models
{
    public class Channel
    {
        public string ID { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string ChannelUri { get; set; }

        [JsonProperty(PropertyName = "channelname")]
        public string ChannelName { get; set; }

        public Channel(string channelName, string channelUri)
        {
            this.ChannelName = channelName;
            this.ChannelUri = channelUri;
        }
    }


    public class Toast
    {
        public static HttpNotificationChannel pushChannel { get; private set; }
        public static IMobileServiceTable<Channel> channelTable = App.MobileService.GetTable<Channel>();

        public static async void AcquirePushChannel()
        {
            // Have to make updates to this to be generated dynamically of the format "{LiveID1}-{LiveID2}-PushChannel"
            string channelName = CreateDynamicChannelName().ToString();

            pushChannel = HttpNotificationChannel.Find(channelName);

            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);
                pushChannel.Open();
                pushChannel.BindToShellToast();
            }

            //Register the channel name and the uri with the Channel table
            await channelTable.InsertAsync(new Channel(channelName, pushChannel.ChannelUri.ToString()));

            //Register for events
            pushChannel.ChannelUriUpdated += pushChannel_ChannelUriUpdated;
            pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

            // Register for this notification only if you need to receive the notifications while your application is running.
            pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

        }

        private static void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            message.AppendFormat("{0}: {1}", e.Collection["wp:Text1"], e.Collection["wp:Text2"]);

            // Display a dialog of all the fields in the toast.
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));

        }

        private static void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }

        static void pushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    e.ChannelUri.ToString()));

            });
        }

        static async Task<string> CreateDynamicChannelName()
        {
            string myID = await InstallationViewModel.GetUniqueServerID(IsolatedStorageSettings.ApplicationSettings["PhoneNumber"].ToString());
            //await InstallationViewModel.GetUniqueServerID(IsolatedStorageSettings.ApplicationSettings["BuddyID"].ToString());
            string buddyID = "5658658C-7331-4102-8A51-0740FFDA77D8";

            string channelName = myID + buddyID;
            string reverseChannelName = buddyID + myID;

            //Check if this ID exists in reverse form in the Channel table
            IMobileServiceTableQuery<string> query = channelTable.Where(channelItem => channelItem.ChannelName == reverseChannelName)
                                                                 .Select(channelItem => channelItem.ChannelUri);

            IEnumerable<string> resultSet = await channelTable.ReadAsync<string>(query);

            if (resultSet.FirstOrDefault() == null)
                return channelName;

            else
                return reverseChannelName;
        }
    }
}
