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
    public class DisplayHistory
    {
        public string Amount
        {
            get
            {
                if (transaction != null)
                {
                    if(transaction.Withdrawal)
                        return "-" + transaction.Amount;
                    return transaction.Amount;
                }
                else return "Amount";
            }
        }
        public string Ledger
        {
            get { if (transaction != null) return transaction.Ledger; else return "Ledger"; }
        }
        
        public string Date
        {
            get { if (transaction != null) return transaction.Date.Year + "-" + transaction.Date.Month + "-" + transaction.Date.Day; else return "Date"; }
        }

        public string Description
        {
            get { if (transaction != null) return transaction.Description; else return "Description"; }
        }

        RedstoneAPI.Transaction transaction;

        public DisplayHistory(RedstoneAPI.Transaction t)
        {
            transaction = t;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionsPage : Page
    {
        public TransactionsPage()
        {
            this.InitializeComponent();
#if WINDOWS_PHONE_APP
            this.backButton.Visibility = Visibility.Collapsed;
#endif
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.progressRing.IsActive = true;
            this.loadingText.Visibility = Visibility.Visible;

            RedstoneAPI.Account account = (RedstoneAPI.Account)e.Parameter;

            var history = await getHistory(account);
            var displayData = new DisplayHistory[history.Length + 1];

            displayData[0] = new DisplayHistory(null);
            for (var i = 0; i < history.Length; i++)
            {
                displayData[i + 1] = new DisplayHistory(history[i]);
            }

            this.transactionList.ItemsSource = displayData;

            this.progressRing.IsActive = false;
            this.loadingText.Visibility = Visibility.Collapsed;
        }

        private async Task<RedstoneAPI.Transaction[]> getHistory(RedstoneAPI.Account account)
        {
            RedstoneAPI.Transaction[] ret = null;
            await Task.Run(() =>
            {
                try
                {
                    ret = account.GetHistory(App.session, new DateTime(1, 1, 1), DateTime.Now);
                }
                catch
                {

                }
            });

            return ret;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        
    }
}
