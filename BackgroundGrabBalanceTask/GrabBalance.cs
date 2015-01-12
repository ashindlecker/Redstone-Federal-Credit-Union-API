using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundGrabBalanceTask
{
    public sealed class GrabBalance : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            
            await GetBalance();


            // Inform the system that the task is finished.
            deferral.Complete();
        }

        public async Task GetBalance()
        {
            await Task.Run(() =>
            {

                string tileXmlString =
                  "<tile>"
                  + "<visual version='3'>"
                  + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
                  + "<text id='1'>Balance: ButtNUtts</text>"
                  + "</binding>"
                  + "<binding template='TileWide310x150Text03' fallback='TileWideText03'>"
                  + "<text id='1'>Hello World! My very own tile notification</text>"
                  + "</binding>"
                  + "<binding template='TileSquare310x310Text09'>"
                  + "<text id='1'>Hello World! My very own tile notification</text>"
                  + "</binding>"
                  + "</visual>"
                  + "</tile>";

                Windows.Data.Xml.Dom.XmlDocument tileDOM = new Windows.Data.Xml.Dom.XmlDocument();

                // Load the xml string into the DOM.
                tileDOM.LoadXml(tileXmlString);
                TileNotification not = new TileNotification(tileDOM);

                var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
                tileUpdater.Clear();
                tileUpdater.Update(not);
            });
        }
    }
}
