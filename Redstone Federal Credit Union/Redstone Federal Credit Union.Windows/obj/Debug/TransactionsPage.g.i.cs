﻿

#pragma checksum "C:\Users\Austin\documents\visual studio 2015\Projects\Redstone Federal Credit Union\Redstone Federal Credit Union\Redstone Federal Credit Union.Shared\TransactionsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "73B7944ADBEB934CADE0043A4ECAD4A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Redstone_Federal_Credit_Union
{
    partial class TransactionsPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.DataTemplate itemTemplate; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton backButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView transactionList; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing progressRing; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock loadingText; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///TransactionsPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            itemTemplate = (global::Windows.UI.Xaml.DataTemplate)this.FindName("itemTemplate");
            backButton = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("backButton");
            transactionList = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("transactionList");
            progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("progressRing");
            loadingText = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("loadingText");
        }
    }
}



