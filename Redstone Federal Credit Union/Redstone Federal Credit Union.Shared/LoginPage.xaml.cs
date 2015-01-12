using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Redstone_Federal_Credit_Union
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void login_click(object sender, RoutedEventArgs e)
        {
            errorText.Visibility = Visibility.Collapsed;
            progressRing.IsActive = true;
            button.IsEnabled = false;
            loadingText.Visibility = Visibility.Visible;
            userText.IsEnabled = false;
            passText.IsEnabled = false;
            await loginAsync(userText.Text, passText.Password);
            userText.IsEnabled = true;
            passText.IsEnabled = true;
            loadingText.Visibility = Visibility.Collapsed;

            progressRing.IsActive = false;

            if(App.session.State == RedstoneAPI.Session.SessionStates.ErrorOccured)
            {
                errorText.Visibility = Visibility.Visible;
                passText.Password = "";
                button.IsEnabled = true;
            }
            else if (App.session.State == RedstoneAPI.Session.SessionStates.WaitingForIdentity)
            {
                this.Frame.Navigate(typeof(ConfirmationPage));
            }
            else if(App.session.State == RedstoneAPI.Session.SessionStates.LoggedIn)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["MAF"] = App.session.GetMAF();
                this.Frame.Navigate(typeof(MemberPage));
            }
        }

        private async Task loginAsync(string username, string password)
        {
            await Task.Run(() =>
            {
                try
                {
                    App.session.Login(username, password);
                }
                catch
                {
                }
            });
        }
    }
}
