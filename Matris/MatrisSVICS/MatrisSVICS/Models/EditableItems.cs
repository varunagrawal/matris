using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    class EditableItems : INotifyPropertyChanged
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }

        public EditableItems(string itemName, int quantity)
        {
	        this.ItemName = itemName;
            this.Quantity = quantity;
            this.IsSelected = false;
        }

        public EditableItems(ObservableCollection<Items> list)
        {
            foreach (Items item in list)
            {
                this.ItemName = item.ItemName;
                this.Quantity = item.Quantity;
                this.IsSelected = false;
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
