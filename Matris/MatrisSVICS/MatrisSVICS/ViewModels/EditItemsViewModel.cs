using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MatrisSVICS.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Runtime.Serialization;
using MatrisSVICS.Helper;



namespace MatrisSVICS.ViewModels
{
    class EditItemsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EditableItems> CurrentItemList { get; set; }

        IsolatedStorageForItems isft;

        private ObservableCollection<EditableItems> _editedItemList;
        public ObservableCollection<EditableItems> EditedItemList
        {
            get { return _editedItemList; }

            set
            {
                _editedItemList = value;
                NotifyPropertyChanged("EditedItemList");
            }
        }

        private ICommand _saveEditedListCommand;
        [IgnoreDataMember]
        public ICommand SaveEditedListCommand
        {
            get { return _saveEditedListCommand; }
        }

        private ICommand _deleteCurrentItemCommand;
        [IgnoreDataMember]
        public ICommand DeleteCurrentItemCommand
        {
            get { return _deleteCurrentItemCommand; }
        }

        private ICommand _cancelOperationCommand;
        [IgnoreDataMember]
        public ICommand CancelOperationCommand
        {
            get { return _cancelOperationCommand; }
        }

        public EditItemsViewModel(object p)
        {
            isft = new IsolatedStorageForItems();
            CurrentItemList = new ObservableCollection<EditableItems>();
            EditedItemList = new ObservableCollection<EditableItems>();
            CurrentItemList = ConvertToEditableItemsModel(((ItemTrackerViewModel)p).ItemList);
            EditedItemList = ConvertToEditableItemsModel(((ItemTrackerViewModel)p).ItemList);


            _saveEditedListCommand = new DelegateCommand(ClickSave);
            _deleteCurrentItemCommand = new DelegateCommand(ClickDelete);
            _cancelOperationCommand = new DelegateCommand(ClickCancel);
            //indexes = new List<int>();
        }



        public void ClickSave(object p)
        {
            // CopyToStorage(EditedItemList);
            PhoneApplicationService.Current.State["PreviousModel"] = this;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
        }

        public void ClickDelete(object p)
        {
            MessageBoxResult result = MessageBox.Show("Delete selected items?", "Confirm", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                foreach (EditableItems item in EditedItemList.ToArray<EditableItems>())
                {
                    if (item.IsSelected)
                        EditedItemList.RemoveAt(EditedItemList.IndexOf(item));
                }
                //  CopyToStorage(EditedItemList);
                PhoneApplicationService.Current.State["PreviousModel"] = this;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
            }
            //  CopyToStorage(EditedItemList);
            //if(result == MessageBoxResult.OK)
            //{
            //    foreach (int index in indexes)
            //        EditedItemList.RemoveAt(index);
            //    MessageBox.Show(EditedItemList.ElementAt(_currentIndex).ItemName);
            //}
        }

        public void ClickCancel(object p)
        {
            MessageBoxResult result =
                MessageBox.Show("Are you sure you want to discard the edited list?", "Confirm cancellation", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                EditedItemList.Clear();
                EditedItemList = Clone(CurrentItemList);
                PhoneApplicationService.Current.State["PreviousModel"] = this;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/BaseItemsPage.xaml", UriKind.Relative));
            }
            //CopyToStorage(EditedItemList);
        }

        private static ObservableCollection<EditableItems> ConvertToEditableItemsModel(ObservableCollection<Items> originalList)
        {
            ObservableCollection<EditableItems> copyList = new ObservableCollection<EditableItems>();
            foreach (Items item in originalList)
                copyList.Add(new EditableItems(item.ItemName, item.Quantity));

            return copyList;
        }

        private static ObservableCollection<EditableItems> Clone(ObservableCollection<EditableItems> originalList)
        {
            ObservableCollection<EditableItems> copyList = new ObservableCollection<EditableItems>();
            foreach (EditableItems item in originalList)
                copyList.Add(new EditableItems(item.ItemName, item.Quantity));

            return copyList;
        }


        //public void AddToListOfCheckedItems(int index)
        //{
        //    if (indexes.Contains(index)) ;
        //    else
        //        indexes.Add(index);
        //}

        //public void RemoveFromListOfCheckedItems(int index)
        //{
        //    if (!indexes.Contains(index)) ;
        //    else
        //        indexes.RemoveAt(indexes.IndexOf(index));
        //}

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
