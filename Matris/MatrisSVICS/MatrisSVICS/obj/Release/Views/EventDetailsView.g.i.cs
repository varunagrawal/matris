﻿#pragma checksum "D:\Matris C4H\Matris\MatrisSVICS\MatrisSVICS\Views\EventDetailsView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BFDD896B889C659D902CB716350C21E9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MatrisSVICS.Calendar {
    
    
    public partial class EventDetailsView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama eventDetails;
        
        internal System.Windows.Controls.TextBlock eventName;
        
        internal System.Windows.Controls.TextBlock ageOfChildYearsNew;
        
        internal System.Windows.Controls.TextBlock ageOfChildMonthNew;
        
        internal System.Windows.Controls.TextBlock ageOfChildDayNew;
        
        internal System.Windows.Controls.TextBlock eventDescription;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MatrisSVICS;component/Views/EventDetailsView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.eventDetails = ((Microsoft.Phone.Controls.Panorama)(this.FindName("eventDetails")));
            this.eventName = ((System.Windows.Controls.TextBlock)(this.FindName("eventName")));
            this.ageOfChildYearsNew = ((System.Windows.Controls.TextBlock)(this.FindName("ageOfChildYearsNew")));
            this.ageOfChildMonthNew = ((System.Windows.Controls.TextBlock)(this.FindName("ageOfChildMonthNew")));
            this.ageOfChildDayNew = ((System.Windows.Controls.TextBlock)(this.FindName("ageOfChildDayNew")));
            this.eventDescription = ((System.Windows.Controls.TextBlock)(this.FindName("eventDescription")));
        }
    }
}

