using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
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
    public sealed partial class MemberPage : Page
    {
        public MemberPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            List<string> cookienames = new List<string>();
            List<string> cookievalues = new List<string>();
            List<string> cookiepaths = new List<string>();
            var cookies = App.session.GetCookies();
            foreach(Cookie cookie in cookies)
            {
                cookienames.Add(cookie.Name);
                cookievalues.Add(cookie.Value);
                cookiepaths.Add(cookie.Path);
            }
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["cookienames"] = cookienames.ToArray();
            localSettings.Values["cookievalues"] = cookievalues.ToArray();
            localSettings.Values["cookiepaths"] = cookiepaths.ToArray();
            localSettings.Values["csrftoken"] = App.session.CSRFTToken;
            localSettings.Values["rftoken"] = App.session.RFToken;

            startBackgroundTask();
            if (App.accounts == null)
            {
                App.accounts = RedstoneAPI.Account.GetAccounts(App.session);
                UpdateLiveTile();
            }
            this.accountsList.ItemsSource = App.accounts;
        }

        public void UpdateLiveTile()
        {
            string tileXmlString =
              "<tile>"
              + "<visual version='3'>"
              + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
              + "<text id='1'></text>"
              + "<text id='2'></text>"
              + "</binding>"

              + "<binding template='TileWide310x150Text03' fallback='TileWideText03'>"
              + "<text id='1'></text>"
              + "<text id='2'></text>"
              + "</binding>"

              + "<binding template='TileSquare310x310Text09'>"
              + "<text id='1'></text>"
              + "<text id='2'></text>"
              + "</binding>"

              + "</visual>"
              + "</tile>";


            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true);
            tileUpdater.Clear();

            XmlDocument tileDOM = new XmlDocument();
            tileDOM.LoadXml(tileXmlString);

            for (var i = 0; i < App.accounts.Length; i++)
            {
                var nodes = tileDOM.SelectNodes("//text[@id='1']");
                foreach (IXmlNode node in nodes)
                {
                    node.InnerText = App.accounts[i].AccountType + ": " + App.accounts[i].Balance;
                }
                TileNotification not = new TileNotification(tileDOM);
                tileUpdater.Update(not);
            }
        }

        private void accountSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(TransactionsPage), App.accounts[accountsList.SelectedIndex]);
        }

        /// <summary>
        /// Background task for updating the live tile with balances
        /// </summary>
        private void startBackgroundTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "Grab Balance Task")
                {
                    return;
                }
            }

            var builder = new BackgroundTaskBuilder();
            builder.Name = "Grab Balance Task";
            builder.TaskEntryPoint = "BackgroundBalanceTask.GrabBalanceTask";
            builder.SetTrigger(new TimeTrigger(15, true));
            builder.Register();
        }
    }
}
