using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RedstoneAPI
{
    public class Transaction
    {
        public JObject JsonObject;

        public string Description
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public string Amount
        {
            get;
            private set;
        }

        public string Type
        {
            get;
            private set;
        }

        public bool Deposit
        {
            get
            {
                return (Type == "MISCELLANEOUS_CREDIT" || Type == "POS_CREDIT");
            }
        }

        public bool Withdrawal
        {
            get
            {
                return !Deposit;
            }
        }

        public string Ledger
        {
            get;
            private set;
        }

        public Transaction(JObject jobj)
        {
            JsonObject = jobj;
            ParseJson(JsonObject);
        }

        public void ParseJson(JObject json)
        {
            Amount = double.Parse(json["amount"].ToString()).ToString("C");
            Description = json["generatedDescription"].ToString();
            Type = json["transactionType"].ToString();
            Ledger = double.Parse(json["ledgerBalance"].ToString()).ToString("C");

            string date = json["transactionDate"].ToString();
            string month = "" + date[0] + date[1];
            string day = "" + date[3] + date[4];
            string year = "" + date[6] + date[7] + date[8] + date[9];

            Date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
    }
}
