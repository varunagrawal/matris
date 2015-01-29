using MatrisSVICS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MatrisSVICS.Helper
{
    public class IsolatedStorageForItems
    {
        //create xml
        public ObservableCollection<Items> createXML(ObservableCollection<Items> listOfItems)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();


            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.FileExists("ItemsStorage21.xml"))
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("ItemsStorage21.xml", FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Items>));
                        //if (!(listOfItems.Count == 0))
                        //    serializer.Serialize(stream, listOfItems);

                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            if (!(listOfItems.Count == 0))
                                serializer.Serialize(xmlWriter, listOfItems);
                        }
                    }
                }
                else
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("ItemsStorage21.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Items>));
                        //  serializer.Serialize(stream, listOfItems);
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {

                            serializer.Serialize(xmlWriter, listOfItems);
                        }
                    }

                }
            }
            return listOfItems;

        }
        public ObservableCollection<Items> readXML(ObservableCollection<Items> listOfItems)
        {
            ObservableCollection<Items> data = new ObservableCollection<Items>();
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!myIsolatedStorage.FileExists("ItemsStorage21.xml"))
                    {
                        data = createXML(listOfItems);
                    }
                    else
                    {
                        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("ItemsStorage21.xml", FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Items>));
                            //  using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                            //   {

                            data = (ObservableCollection<Items>)serializer.Deserialize(stream);
                            //  }

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
