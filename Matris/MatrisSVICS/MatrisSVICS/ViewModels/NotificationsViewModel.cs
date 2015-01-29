using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MatrisSVICS.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
using MatrisSVICS.Helper;

namespace MatrisSVICS.ViewModels
{
    public class NotificationsViewModel : INotifyPropertyChanged
    {
        //added 
        PullNotificationStorage pns;
        ObservableCollection<CompositeNotification> _toBePushed;
        public ObservableCollection<CompositeNotification> ToBePushed
        {
            get
            {
                return _toBePushed;
            }

            set
            {
                _toBePushed = value;
                NotifyPropertyChanged("ToBePushed");
            }
        }

        ObservableCollection<CompositeNotification> _cloudItemsList;
        public ObservableCollection<CompositeNotification> CloudItemsList
        {
            get
            {
                return _cloudItemsList;
            }

            set
            {
                _cloudItemsList = value;
                NotifyPropertyChanged("CloudItemsList");
            }
        }

        ObservableCollection<CompositeNotification> _xmlItemsList;
        public ObservableCollection<CompositeNotification> XMLItemsList
        {
            get
            {
                return _xmlItemsList;
            }

            set
            {
                _xmlItemsList = value;
                NotifyPropertyChanged("XMLItemsList");
            }
        }


        IMobileServiceTable<Notifications> notificationsTable = App.MobileService.GetTable<Notifications>();
        private ICommand _getCloudDataCommand;

        [IgnoreDataMember]
        public ICommand GetCloudDataCommand
        {
            get { return _getCloudDataCommand; }
        }

        private ICommand _markAsDoneCommand;
        [IgnoreDataMember]
        public ICommand MarkAsDoneCommand
        {
            get { return _markAsDoneCommand; }
        }

        public NotificationsViewModel()
        {
            pns = new PullNotificationStorage();
            _getCloudDataCommand = new DelegateCommand(ClickRefresh);
            _markAsDoneCommand = new DelegateCommand(ClickDelete);
        }

        public NotificationsViewModel(Items currentlySelectedItem, int quantityRequired, string optionalMessage)
        {
            PushToCloud(currentlySelectedItem, quantityRequired, optionalMessage);
        }

        public async void PushToCloud(Items currentlySelectedItem, int quantityRequired, string optionalMessage)
        {
            //'From' tag has to be populated with user's name
            string from = IsolatedStorageSettings.ApplicationSettings["UserName"].ToString();

            //Get currently configured buddy contact
            //string buddyContactID = IsolatedSettings.ApplicationSettings["BuddyContactID"];
            string buddyContactID = IsolatedStorageSettings.ApplicationSettings["BuddyValue"].ToString();


            //Create new notification and push to cloud
            Notifications itemToPush = new Notifications(from, buddyContactID, currentlySelectedItem, quantityRequired, optionalMessage);
            try
            {
                await notificationsTable.InsertAsync(itemToPush);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public async void ClickRefresh(object p)
        {


            //  string deviceUniqueID = await InstallationViewModel.GetUniqueServerID(IsolatedStorageSettings.ApplicationSettings["PhoneNumber"].ToString());
            //   string deviceUniqueID = "C59D59EA-6B7F-4AC3-9520-35818B423011";

            string deviceUniqueID = IsolatedStorageSettings.ApplicationSettings["PhoneNumber"].ToString();
            IMobileServiceTableQuery<Notifications> query =
                notificationsTable.Where(notificationsItem => notificationsItem.To == deviceUniqueID);

            IEnumerable<Notifications> list = await notificationsTable.ReadAsync<Notifications>(query);

            ObservableCollection<Notifications> newlist = new ObservableCollection<Notifications>(list);

            ObservableCollection<Notifications> oldlist = this.GetXMLItemsList();
            if (oldlist.Count == 1)
            {
                if (oldlist[0].From.ToString() == "")
                {
                    oldlist.RemoveAt(0);
                }
            }
            XMLItemsList = this.AddIcons(oldlist, "old");

            ObservableCollection<Notifications> filteredlist = deleteInCloud(newlist, oldlist).Result;

            CloudItemsList = this.AddIcons(filteredlist, "new");

            ToBePushed = new ObservableCollection<CompositeNotification>(CloudItemsList.Union(XMLItemsList));
            ModifyToXML(filteredlist);
            deleteNewElements(filteredlist);

        }
        public async Task<ObservableCollection<Notifications>> deleteInCloud(ObservableCollection<Notifications> newlist, ObservableCollection<Notifications> oldlist)
        {
            try
            {


                for (int k = 0; k < newlist.Count; k++)
                {
                    for (int i = 0; i < oldlist.Count; i++)
                    {

                        if (newlist[k].ChannelName == oldlist[i].ChannelName && newlist[k].From == oldlist[i].From && newlist[k].ItemName == oldlist[i].ItemName && newlist[k].OptionalMessage == oldlist[i].OptionalMessage && newlist[k].Quantity == oldlist[i].Quantity && newlist[k].To == oldlist[i].To)
                        {
                            Notifications ks = (Notifications)newlist[k];
                            newlist.Remove((Notifications)newlist[k]);


                            try
                            {
                                await notificationsTable.DeleteAsync(ks);
                            }
                            catch (MobileServiceInvalidOperationException e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message);
                            }
                            k--;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return newlist;
        }
        void ModifyToXML(ObservableCollection<Notifications> enList)
        {
            ObservableCollection<Notifications> listxml = this.GetXMLItemsList();
            ObservableCollection<Notifications> xmlist = new ObservableCollection<Notifications>();
            //  ObservableCollection<Notifications> Oblist = new ObservableCollection<Notifications>(enList);
            if (listxml.Count == 1)
            {
                if (listxml[0].From.ToString() == "")
                {
                    xmlist = enList;
                }
                else
                {
                    xmlist = ToObservableCollection(enList, listxml);
                    //var unitedPoints = new ObservableCollection<Notifications>(); ;
                    //foreach (var p in enList.Union(listxml))
                    //    unitedPoints.Add(p);
                    //xmlist = new ObservableCollection<Notifications>(unitedPoints);


                }
            }
            else
            {
                xmlist = ToObservableCollection(enList, listxml);
                ////   enList.Concat(listxml);
                //   var unitedPoints = new ObservableCollection<Notifications>(); ;
                //   foreach (var p in enList.Union(listxml))
                //       unitedPoints.Add(p);
                //   xmlist = new ObservableCollection<Notifications>(unitedPoints);
            }

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists("Notifications27.xml")) { myIsolatedStorage.DeleteFile("Notifications27.xml"); }
            }
            pns.createXML(xmlist);

        }
        public void deleteNewElements(ObservableCollection<Notifications> itemlist)
        {
            for (int k = 0; k < itemlist.Count; k++)
            {
                notificationsTable.DeleteAsync((Notifications)itemlist[k]);

            }

        }

        public ObservableCollection<Notifications> ToObservableCollection<Notifications>(ObservableCollection<Notifications> newl, ObservableCollection<Notifications> old)
        {
            // ObservableCollection<Notifications> collection = new ObservableCollection<Notifications>();

            foreach (var item in old)
            {
                newl.Add(item);
            }

            return newl;
        }

        private ObservableCollection<CompositeNotification> AddIcons(IEnumerable<Notifications> originalList, string oldOrNew)
        {
            ObservableCollection<CompositeNotification> compositeList = new ObservableCollection<CompositeNotification>();
            foreach (Notifications item in originalList)
            {
                compositeList.Add(new CompositeNotification(item, oldOrNew));
            }

            return compositeList;
        }

        private ObservableCollection<Notifications> GetXMLItemsList()
        {
            /** Code to get data from 
             * XML, yet to be integrated
             * */

            ObservableCollection<Notifications> oldItemsList = new ObservableCollection<Notifications>();


            //   pns.createXML(oldItemsList);
            ObservableCollection<Notifications> emptylist = new ObservableCollection<Notifications>();

            emptylist.Add(new Notifications("", "", new Items("", 0), 0, ""));
            //  emptylist.Add(new Notifications("chaitra", "yyy", new Items("biskets", 7), 5, "Johnson'schcosoosl"));
            oldItemsList = pns.readXML(emptylist);
            return oldItemsList;
        }

        private void ClickDelete(object p)
        {
            ObservableCollection<Notifications> getList = GetXMLItemsList();
            MessageBoxResult result = MessageBox.Show("Delete notifications?", "Confirm deletion", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                foreach (CompositeNotification item in ToBePushed.ToArray<CompositeNotification>())
                {
                    if (item.IsMarkedDone)
                    {

                        ToBePushed.RemoveAt(ToBePushed.IndexOf(item));
                        var st = getList.Where(s => s.ItemName == item.NotificationItem.ItemName).ToList();
                        foreach (var itemremove in st)
                        {
                            getList.Remove(itemremove);
                        }



                    }
                }
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists("Notifications27.xml")) { myIsolatedStorage.DeleteFile("Notifications27.xml"); }
                }
                pns.createXML(getList);
            }
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
