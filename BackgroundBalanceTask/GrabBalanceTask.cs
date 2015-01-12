using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundBalanceTask
{
    public sealed class GrabBalanceTask : IBackgroundTask
    {
        private RedstoneAPI.Session session;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();


            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            
            session = new RedstoneAPI.Session();
            string[] cookieNames = (string[])localSettings.Values["cookienames"];
            string[] cookieValues = (string[])localSettings.Values["cookievalues"];
            string[] cookiePaths = (string[])localSettings.Values["cookiepaths"];
            for(var i = 0; i < cookieNames.Length; i++)
            {
                session.Handler.CookieContainer.Add(new Uri("https://www.redfcuonline.org"), new System.Net.Cookie(cookieNames[i], cookieValues[i], "/"));
            }

            session.RFToken = (string)localSettings.Values["rftoken"];
            session.CSRFTToken = (string)localSettings.Values["csrftoken"];
            session.State = RedstoneAPI.Session.SessionStates.LoggedIn;


            await GetBalance();

            deferral.Complete();
        }

        private async Task GetBalance()
        {
            RedstoneAPI.Account[] accounts = null;
            await Task.Run(() =>
            {
                accounts = RedstoneAPI.Account.GetAccounts(session);
            });

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

            for (var i = 0; i < accounts.Length; i++)
            {
                var nodes = tileDOM.SelectNodes("//text[@id='1']");
                foreach (IXmlNode node in nodes)
                {
                    node.InnerText = accounts[i].AccountType + ": " + accounts[i].Balance;
                }
                TileNotification not = new TileNotification(tileDOM);
                tileUpdater.Update(not);
            }

        }
    }
}
