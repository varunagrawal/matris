using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MatrisSVICS.Models
{
    public class HealthModel : INotifyPropertyChanged
    {
        private string _id;
        /// <summary>
        /// Unique object ID
        /// </summary>
        /// <returns></returns>
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private string _pageName;
        /// <summary>
        /// Stores the name of the Health Page in the blob storage
        /// </summary>
        /// <returns></returns>
        public string PageName
        {
            get
            {
                return _pageName;
            }
            set
            {
                if (value != _pageName)
                {
                    _pageName = value;
                    NotifyPropertyChanged("PageName");
                }
            }
        }

        private string _pageLink;
        /// <summary>
        /// Stores the link to the Health Web Page in the Blob storage
        /// </summary>
        /// <returns></returns>
        public string PageLink
        {
            get
            {
                return _pageLink;
            }
            set
            {
                if (value != _pageLink)
                {
                    _pageLink = value;
                    NotifyPropertyChanged("PageLink");
                }
            }
        }

        private string _category;
        /// <summary>
        /// Stores the category of the Health Web Page in the Blob storage
        /// </summary>
        /// <returns></returns>
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (value != _category)
                {
                    _category = value;
                    NotifyPropertyChanged("PageLink");
                }
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

        public static ObservableCollection<HealthModel> PagesList = null;

        public static ObservableCollection<HealthModel> GetPagesList()
        {
            if (PagesList == null)
            {
                PagesList = GetPagesListFromFile();    
            }

            return PagesList;

        }

        private static ObservableCollection<HealthModel> GetPagesListFromFile()
        {
            PagesList = new ObservableCollection<HealthModel>();
            string[] Links;

            try
            {
                // Get the file.
                var file = File.OpenRead("Models/HealthPagesLinks.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    Links = streamReader.ReadToEnd().Split(new char[] { '\n' });
                }

                for (int i = 0; i < Links.Length; i++)
                {
                    HealthModel page = new HealthModel();
                    page.ID = i.ToString();
                    page.Category = Links[i].Split(new char[] { ';' })[0].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    page.PageName = Links[i].Split(new char[] { ';' })[1].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    page.PageLink = Links[i].Split(new char[] { ';' })[2].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    PagesList.Add(page);
                }

                return PagesList;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
