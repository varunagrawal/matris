using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MatrisSVICS.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System.Runtime.Serialization;
using MatrisSVICS.Helper;
using System.IO.IsolatedStorage;


namespace MatrisSVICS.ViewModels
{
    public class ItemTrackerViewModel : INotifyPropertyChanged
    {
        #region Properties
        private Items _currentlySelectedItem;
        public IsolatedStorageForItems isfts;
        public ContactsViewModel conVM;
        public Task<Int32> result;

        [IgnoreDataMember]
        public ObservableCollection<Items> ItemList { get; set; }


        public int QuantityRequired { get; set; }
        public string OptionalMessage { get; set; }

        private bool _isPopupEnabled;
        [IgnoreDataMember]
        public bool IsPopupEnabled
        {
            get
            {
                return _isPopupEnabled;
            }

            set
            {
                if (_isPopupEnabled == value) return;
                _isPopupEnabled = value;
                NotifyPropertyChanged("IsPopupEnabled");
            }
        }

        [IgnoreDataMember]
        public Items CurrentlySelectedItem
        {
            get { return _currentlySelectedItem; }
            set
            {
                _currentlySelectedItem = value;
                InitiateCloudTransfer();
            }
        }

        private ICommand _addItemsCommand, _editItemsCommand, _menuItemsCommand;
        [IgnoreDataMember]
        public ICommand AddItemsCommand
        {
            get
            {
                return _addItemsCommand;
            }
        }

        [IgnoreDataMember]
        public ICommand EditItemsCommand
        {
            get
            {
                return _editItemsCommand;
            }
        }

        //added
        [IgnoreDataMember]
        public ICommand MenuItemsCommand
        {
            get
            {
                return _menuItemsCommand;
            }
        }


        private ICommand _pushToCloudCommand;
        [IgnoreDataMember]
        public ICommand PushToCloudCommand
        {
            get
            {
                return _pushToCloudCommand;
            }
        }

        private ICommand _cancelCommand;
        [IgnoreDataMember]
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
        }

        //private string _isEmptyListText;

        //public string EmptyListText
        //{
        //    get 
        //    {
        //        return _isEmptyListText;
        //    }

        //    set
        //    {
        //        if (_isEmptyListText == value) return;
        //        _isEmptyListText = value;
        //        NotifyPropertyChanged("EmptyListText");
        //    }
        //}

        //private Visibility _isListEmpty;

        //public Visibility ListEmpty
        //{
        //    get
        //    {
        //        return _isListEmpty;
        //    }

        //    set
        //    {
        //        if (_isListEmpty == value) return;
        //        _isListEmpty = value;
        //        NotifyPropertyChanged("ListEmpty");
        //    }
        //}


        //addd
        private Visibility _isListEnabled;
        [IgnoreDataMember]
        public Visibility IsListEnabled
        {
            get
            {
                return _isListEnabled;
            }

            set
            {
                if (_isListEnabled == value) return;
                _isListEnabled = value;
                NotifyPropertyChanged("IsListEnabled");
            }
        }

        #endregion




        public ItemTrackerViewModel(object p)
        {
            conVM = new ContactsViewModel();
            result = conVM.RefreshTodoItems();
            isfts = new IsolatedStorageForItems();
            _addItemsCommand = new DelegateCommand(ClickAdd);
            _editItemsCommand = new DelegateCommand(ClickEdit);
            _pushToCloudCommand = new DelegateCommand(ClickOK);
            _cancelCommand = new DelegateCommand(ClickCancel);
            //added
            _menuItemsCommand = new DelegateCommand(ClickMenu);

            //Check where this page has been navigated to, from:


            //this part is commented

            //After editing items
            if (p is EditItemsViewModel)
            {
                ItemList = ConvertToItemsModel(((EditItemsViewModel)p).EditedItemList);
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists("ItemsStorage21.xml")) { myIsolatedStorage.DeleteFile("ItemsStorage21.xml"); }
                }
                isfts.createXML(ItemList);
                //  ObservableCollection<Items> st =   isfts.readXML(ItemList);
            }

            //After adding new items
            else if (p is AddItemsViewModel)
            {
                ItemList = ((AddItemsViewModel)p).NewItemList;
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists("ItemsStorage21.xml")) { myIsolatedStorage.DeleteFile("ItemsStorage21.xml"); }
                }
                isfts.createXML(ItemList);
            }

            //After installation page
            else
                this.AddDefaultItems();
            // isfts.createXML(ItemList);

        }

        public void AddDefaultItems()
        {

            ObservableCollection<Items> list = new ObservableCollection<Items>();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Defaultitems"))
            {
                list.Add(new Items("", 0));

            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["Defaultitems"] = true;

                list.Add(new Items("Baby shampoo", 1));
                list.Add(new Items("Baby soap", 1));
                list.Add(new Items("Soft towels", 4));
                list.Add(new Items("Washcloths", 2));
                list.Add(new Items("Nail clipper", 1));
                list.Add(new Items("Diapers", 10));
                list.Add(new Items("Pacifier", 2));
                list.Add(new Items("Bottles", 3));
                list.Add(new Items("Baby formula", 1));
            }
            list = isfts.readXML(list);
            if (list.Count == 0)
            {
                //ListEmpty = Visibility.Visible;
                //EmptyListText = "No New Items are added";
                ItemList = list;
            }
            else
            {
                ItemList = list;
                IsListEnabled = Visibility.Visible;
            }



        }

        public void InitiateCloudTransfer()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("BuddyValue"))
            {
                pushItemsCheck();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Please select buddy to send Notification from contacts Menu", "Add Buddy", MessageBoxButton.OK);
            }
        }
        public void pushItemsCheck()
        {
            MessageBoxResult result = MessageBox.Show(string.Concat("Push item '", CurrentlySelectedItem.ItemName, "' to the cloud?"), "Sync", MessageBoxButton.OKCancel);
            if (result.Equals(MessageBoxResult.OK))
            {
                IsListEnabled = Visibility.Collapsed;
                IsPopupEnabled = true;
            }
        }


        public void ClickAdd(object p)
        {
            PhoneApplicationService.Current.State["CurrentModel"] = this;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/AddItemsPage.xaml", UriKind.Relative));
        }

        public void ClickEdit(object p)
        {
            PhoneApplicationService.Current.State["CurrentModel"] = this;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/EditItemsPage.xaml", UriKind.Relative));
        }

        public void ClickOK(object p)
        {
            new NotificationsViewModel().PushToCloud(CurrentlySelectedItem, QuantityRequired, OptionalMessage);
            IsPopupEnabled = false;
            IsListEnabled = Visibility.Visible;
        }
        //added
        public void ClickMenu(object p)
        {
            int flag = result.Result;
            if (flag == 1)
                conVM.contactMethod();
            //PhoneApplicationService.Current.State["contactsModel"] = this;
            //  (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/Contacts.xaml", UriKind.Relative));   
        }
        public void ClickCancel(object p)
        {
            IsPopupEnabled = false;
            IsListEnabled = Visibility.Visible;
        }

        private static ObservableCollection<Items> ConvertToItemsModel(ObservableCollection<EditableItems> originalList)
        {
            ObservableCollection<Items> copyList = new ObservableCollection<Items>();
            foreach (EditableItems item in originalList)
            {
                copyList.Add(new Items(item.ItemName, item.Quantity));
            }
            return copyList;
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
