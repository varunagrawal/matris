﻿#pragma checksum "D:\Matris C4H\Matris\MatrisSVICS\MatrisSVICS\Views\AddEventView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B4143C2F79CA91BB04A59221AC1C81D6"
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
    
    
    public partial class AddEventView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox eventNameLabel;
        
        internal System.Windows.Controls.TextBox eventNameNew;
        
        internal System.Windows.Controls.TextBox eventDescriptionNew;
        
        internal System.Windows.Controls.TextBox ageOfChildYearsNew;
        
        internal System.Windows.Controls.TextBox ageOfChildMonthNew;
        
        internal System.Windows.Controls.TextBox ageOfChildDayNew;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MatrisSVICS;component/Views/AddEventView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.eventNameLabel = ((System.Windows.Controls.ListBox)(this.FindName("eventNameLabel")));
            this.eventNameNew = ((System.Windows.Controls.TextBox)(this.FindName("eventNameNew")));
            this.eventDescriptionNew = ((System.Windows.Controls.TextBox)(this.FindName("eventDescriptionNew")));
            this.ageOfChildYearsNew = ((System.Windows.Controls.TextBox)(this.FindName("ageOfChildYearsNew")));
            this.ageOfChildMonthNew = ((System.Windows.Controls.TextBox)(this.FindName("ageOfChildMonthNew")));
            this.ageOfChildDayNew = ((System.Windows.Controls.TextBox)(this.FindName("ageOfChildDayNew")));
        }
    }
}
