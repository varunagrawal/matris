using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MatrisSVICS.Models;
using System.ComponentModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Windows;
using System.Runtime.Serialization;
using MatrisSVICS.Helper;

namespace MatrisSVICS.ViewModels
{
    class AddItemsViewModel
    {
        public ObservableCollection<Items> CurrentItemList { get; set; }
        public ObservableCollection<Items> AddedItemList { get; set; }
        public ObservableCollection<Items> NewItemList { get; set; }
        ObservableCollection<Items> list { get; set; }
        IsolatedStorageForItems isft;

        private ICommand _saveNewListCommand;
        [IgnoreDataMember]
        public ICommand SaveNewListCommand
        {
            get { return _saveNewListCommand; }
        }

        private ICommand _addNextItemCommand;
        [IgnoreDataMember]
        public ICommand AddNextItemCommand
        {
            get { return _addNextItemCommand; }
        }

        private ICommand _cancelOperationCommand;
        [IgnoreDataMember]
        public ICommand CancelOperationCommand
        {
            get { return _cancelOperationCommand; }
        }

        public AddItemsViewModel(object p)
        {
            AddedItemList = new ObservableCollection<Items>();
            this.AddDefaultItems();
            CurrentItemList = ((ItemTrackerViewModel)p).ItemList;
            _saveNewListCommand = new DelegateCommand(ClickSave);
            _addNextItemCommand = new DelegateCommand(ClickAdd);
            _cancelOperationCommand = new DelegateCommand(ClickCancel);
        }

        public void AddDefaultItems()
        {
            isft = new IsolatedStorageForItems();
            list = new ObservableCollection<Items>();
            list.Add(new Items("", 0));
            //aadded
            //    list = isft.readXML(list);
            //    CurrentItemList = list;

            AddedItemList = list;
            // AddedItemList = list;
        }

        public void ClickSave(object p)
        {

            if (AddedItemList.ElementAt<Items>(AddedItemList.Count - 1).ItemName == "")
                AddedItemList.RemoveAt(AddedItemList.Count - 1);

            NewItemList = new ObservableCollection<Items>(CurrentItemList.Concat(AddedItemList));
            //added 
            //    isft.createXML(NewItemList);
            PhoneApplicationService.Current.State["PreviousModel"] = this;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
        }

        public void ClickAdd(object p)
        {
            //If previous input boxes are still empty, then do not add further fields
            if (AddedItemList.ElementAt<Items>(AddedItemList.Count - 1).ItemName == "")
            { }
            else AddedItemList.Add(new Items("", 0));
        }

        public void ClickCancel(object p)
        {
            MessageBoxResult result =
                MessageBox.Show("Are you sure you want to discard this list?", "Confirm cancellation", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                AddedItemList.Clear();
                NewItemList = CurrentItemList;
                PhoneApplicationService.Current.State["PreviousModel"] = this;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
            }
        }
    }
}
