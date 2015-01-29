using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using MatrisSVICS.Models;
using System.Collections.ObjectModel;

namespace MatrisSVICS.ViewModels
{
    public class HealthViewModel : INotifyPropertyChanged
    {
        public HealthViewModel()
        { 
            Items = new ObservableCollection<HealthModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<HealthModel> Items { get; set; }


        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            this.Items = HealthModel.GetPagesList();

            this.IsDataLoaded = true;
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

    public class HealthCategoryViewModel
    {
        private ObservableCollection<string> _categories = new ObservableCollection<string>();

        public ObservableCollection<string> Categories
        {
            get
            {
                return _categories;
            }
            set 
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                _categories = value;
            }
        }
    }
}
