﻿#pragma checksum "D:\Matris C4H\Matris\MatrisSVICS\MatrisSVICS\Views\VideoView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B46A1E35BDF088EDECD7EFD5A91CA89B"
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
using Microsoft.Phone.Shell;
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


namespace MatrisSVICS.Views {
    
    
    public partial class VideoView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Shapes.Rectangle viewfinderRectangle;
        
        internal System.Windows.Controls.MediaElement VideoPlayer;
        
        internal System.Windows.Controls.TextBlock txtDebug;
        
        internal Microsoft.Phone.Shell.ApplicationBar PhoneAppBar;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Playback;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Stop;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Record;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Picture;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MatrisSVICS;component/Views/VideoView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.viewfinderRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("viewfinderRectangle")));
            this.VideoPlayer = ((System.Windows.Controls.MediaElement)(this.FindName("VideoPlayer")));
            this.txtDebug = ((System.Windows.Controls.TextBlock)(this.FindName("txtDebug")));
            this.PhoneAppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("PhoneAppBar")));
            this.Playback = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Playback")));
            this.Stop = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Stop")));
            this.Record = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Record")));
            this.Picture = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Picture")));
        }
    }
}

