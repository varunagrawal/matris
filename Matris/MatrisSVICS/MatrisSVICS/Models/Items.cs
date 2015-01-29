using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Models
{
    public class Items : INotifyPropertyChanged
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public Items()
        {

        }
        public Items(string itemName, int quantity)
        {
	        this.ItemName = itemName;
            this.Quantity = quantity;
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


        /** Implement IDataErrorInfo **/

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        string result = string.Empty;
        //        switch (columnName)
        //        {
        //            case "ItemName": if (string.IsNullOrEmpty(ItemName)) result = "Name is required!"; break;
        //            case "Quantity": if (!(Quantity is int)) result = "Price must be between 10 and 1000"; break;
        //        };
        //        return result;
        //    }
        //}	

        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
