using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ConfirmationPage : Page
    {
        private int OTPId = -1;

        public ConfirmationPage()
        {
            this.InitializeComponent();
            this.callValue.Text = App.session.OTPMethods[0].DestinationDescription;
            this.textValue.Text = App.session.OTPMethods[1].DestinationDescription;
            this.emailValue.Text = App.session.OTPMethods[2].DestinationDescription;
        }

        private async void onEmailTapped(object sender, TappedRoutedEventArgs e)
        {
            await requestCodeAsync(2);
        }

        private async void onCallTapped(object sender, TappedRoutedEventArgs e)
        {
            await requestCodeAsync(0);
        }

        private async void onTextTapped(object sender, TappedRoutedEventArgs e)
        {
            await requestCodeAsync(1);
        }

        private async void onConfirmationSend(object sender, RoutedEventArgs e)
        {
            await sendCodeAsync(confirmationBox.Text);
            if(App.session.State == RedstoneAPI.Session.SessionStates.LoggedIn)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["MAF"] = App.session.GetMAF();
                Frame.Navigate(typeof(MemberPage));
            }
            else
            {
                errorOccuredMessage.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                errorOccuredMessage.Text = "Confirmation code incorrect, please check if typed correctly and try again.";
            }
        }

        private void hideLoadUI()
        {
            loadingText.Visibility = Visibility.Collapsed;
            progressRing.IsActive = false;

            emailSelection.IsTapEnabled = true;
            emailValue.IsTapEnabled = true;
            textSelection.IsTapEnabled = true;
            textValue.IsTapEnabled = true;
            callSelection.IsTapEnabled = true;
            callValue.IsTapEnabled = true;
            confirmationButton.IsEnabled = true; ;

            verificationDescription.Visibility = Visibility.Visible;
            confirmationBox.Visibility = Visibility.Visible;
            confirmationButton.Visibility = Visibility.Visible;
            errorOccuredMessage.Visibility = Visibility.Visible;
        }

        private void loadUI(string text = "loading...")
        {
            errorOccuredMessage.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            errorOccuredMessage.Text = "Didn't receive a code? Wait a bit or select another option";
            verificationDescription.Visibility = Visibility.Collapsed;
            confirmationBox.Visibility = Visibility.Collapsed;
            confirmationButton.Visibility = Visibility.Collapsed;
            errorOccuredMessage.Visibility = Visibility.Collapsed;

            loadingText.Visibility = Visibility.Visible;
            loadingText.Text = text;
            progressRing.IsActive = true;
            emailSelection.IsTapEnabled = false;
            emailValue.IsTapEnabled = false;
            textSelection.IsTapEnabled = false;
            textValue.IsTapEnabled = false;
            callSelection.IsTapEnabled = false;
            callValue.IsTapEnabled = false;
            confirmationButton.IsEnabled = false;
        }

        private async Task sendCodeAsync(string code)
        {
            loadUI("Sending confirmation code...");
            await Task.Run(() =>
            {
                try
                {
                    App.session.SendIdentityCode(code, App.session.OTPMethods[OTPId]);
                }
                catch
                {

                }
            });
            hideLoadUI();
        }

        private async Task requestCodeAsync(int id)
        {
            OTPId = id;

            loadUI("Requesting confirmation code...");

            await Task.Run(() =>
            {
                try
                {
                    App.session.RequestConfirmationCode(App.session.OTPMethods[OTPId]);
                }
                catch
                {

                }
            });

            hideLoadUI();
        }

    }
}
