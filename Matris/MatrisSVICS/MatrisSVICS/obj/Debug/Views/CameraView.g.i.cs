﻿#pragma checksum "D:\Matris C4H\Matris\MatrisApp\MatrisApp\Views\CameraView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6AF80F0F79689B5C7CA63F05A90A8C5B"
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


namespace MatrisApp.Views {
    
    
    public partial class CameraView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas viewfinderCanvas;
        
        internal System.Windows.Media.VideoBrush viewfinderBrush;
        
        internal System.Windows.Controls.TextBlock txtInfo;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Gallery;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton PhotoClick;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MatrisApp;component/Views/CameraView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.viewfinderCanvas = ((System.Windows.Controls.Canvas)(this.FindName("viewfinderCanvas")));
            this.viewfinderBrush = ((System.Windows.Media.VideoBrush)(this.FindName("viewfinderBrush")));
            this.txtInfo = ((System.Windows.Controls.TextBlock)(this.FindName("txtInfo")));
            this.Gallery = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Gallery")));
            this.PhotoClick = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("PhotoClick")));
        }
    }
}

