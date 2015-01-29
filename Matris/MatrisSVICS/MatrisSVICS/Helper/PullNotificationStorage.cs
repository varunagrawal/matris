using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;
using MatrisSVICS.Models;
using System.Collections.ObjectModel;

namespace MatrisSVICS.Helper
{
    public class PullNotificationStorage
    {
        //create xml
        public ObservableCollection<Notifications> createXML(ObservableCollection<Notifications> listOfNotifications)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();

            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.FileExists("Notifications27.xml"))
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Notifications27.xml", FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Notifications>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            if (!(listOfNotifications.Count == 0))
                                serializer.Serialize(xmlWriter, listOfNotifications);
                        }
                    }
                }
                else
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Notifications27.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Notifications>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {

                            serializer.Serialize(xmlWriter, listOfNotifications);
                        }
                    }

                }
            }
            return listOfNotifications;

        }
        public ObservableCollection<Notifications> readXML(ObservableCollection<Notifications> listOfNotifications)
        {
            ObservableCollection<Notifications> data = new ObservableCollection<Notifications>();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!myIsolatedStorage.FileExists("Notifications27.xml"))
                    {
                        //  if (listOfNotifications.Count != 0)
                        // {
                        data = createXML(listOfNotifications);
                        // }
                    }
                    else
                    {
                        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Notifications27.xml", FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Notifications>));

                            data = (ObservableCollection<Notifications>)serializer.Deserialize(stream);

                        }
                    }
                }
            }
            catch (XmlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                //add some code here
            }
            return data;
        }
    }
}
