using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MatrisSVICS.Models;
using MatrisSVICS.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;




namespace MatrisSVICS.ViewModels
{
    public class BaseItemViewModel
    {
        ItemTrackerViewModel _itemTrackerVM;
        public ItemTrackerViewModel ItemTrackerVM
        {
            get
            {
                return _itemTrackerVM;
            }

            set
            {
                _itemTrackerVM = value;
            }
        }

        NotificationsViewModel _notificationsVM;
        public NotificationsViewModel NotificationsVM
        {
            get
            {
                return _notificationsVM;
            }
            set
            {
                _notificationsVM = value;
            }
        }

        public BaseItemViewModel()
        {
            _notificationsVM = new NotificationsViewModel();
            try
            {
                object currentState = PhoneApplicationService.Current.State["PreviousModel"];
                _itemTrackerVM = new ItemTrackerViewModel(currentState);
            }
            catch (Exception e)
            {
                _itemTrackerVM = new ItemTrackerViewModel(null);
                Debug.WriteLine(e.Message);
            }
        }
    }
}