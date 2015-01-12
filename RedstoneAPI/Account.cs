using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft;

namespace RedstoneAPI
{
    public class Account
    {
        public JObject JsonObject;

        public string Id
        {
            get;  set;
        }

        public string Balance
        {
            get;  set;
        }

        public string AccountNumber
        {
            get;  set;
        }

        public string AccountType
        {
            get;  set;
        }

        public Account(JObject jobject)
        {
            JsonObject = jobject;
            ParseJson(JsonObject);
        }

        public void ParseJson(JObject jobject)
        {
            Balance = double.Parse(jobject["balance"].ToString()).ToString("C");
            AccountNumber = jobject["displayAccountNumber"].ToString();
            AccountType = jobject["accountType"].ToString();
            Id = jobject["id"].ToString();
        }

        public Transaction[] GetHistory(Session session, DateTime start, DateTime end)
        {
            List<Transaction> ret = new List<Transaction>();
            Endpoints.GetTransactions.Send(new string[] { session.RFToken, this.Id, GetDateString(start), GetDateString(end) }, session.Client, (HttpResponseMessage response) =>
            {
                JObject obj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                JArray array = JArray.Parse(obj["transactionsresponse"].ToString());

                for(var i = 0; i < array.Count; i++)
                {
                    ret.Add(new Transaction(JObject.Parse(array[i].ToString())));
                }
            });
            return ret.ToArray();
        }

        public static string GetDateString(DateTime date)
        {
            return date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString() + " " + date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString();
        }

        public static Account[] GetAccounts(Session session)
        {
            var accounts = new List<Account>();
            JArray ret = null;
            Endpoints.GetAccounts.Send(new string[] { session.RFToken, "true" }, session.Client, (HttpResponseMessage response) =>
            {
                var json = response.Content.ReadAsStringAsync().Result;
                ret = JArray.Parse(JObject.Parse(json)["accountsresponse"].ToString());

                for(var i = 0; i < ret.Count; i++)
                    accounts.Add(new Account(JObject.Parse(ret[i].ToString())));
            });
            return accounts.ToArray();
        }
    }
}