using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace RedstoneAPI
{
    public class Endpoint
    {
        public delegate void ResponseDelegate(HttpResponseMessage response);
        public event ResponseDelegate OnResponse;

        public String[] Parameters;
        public String Url;
        public String Method;

        private string sendData;

        public Endpoint(string url, String[] param, string method)
        {
            Url = url;
            Parameters = param;
            Method = method;

            sendData = "";
        }

        public void Send(string[] parameters, HttpClient client, ResponseDelegate immediateCallback = null)
        {
            sendData = "";
            for (var i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                    sendData += "&";
                sendData += Parameters[i] + "=" + parameters[i];
            }

            string newUrl = Url;

            if (Method == "GET")
            {
                newUrl += "?" + sendData;
                var response = client.GetAsync(newUrl).Result;

                if (immediateCallback != null)
                    immediateCallback(response);
                if (OnResponse != null)
                    OnResponse(response);
                response.Dispose();
            }
            else
            {
                var pairArray = new List<KeyValuePair<string, string>>();
                for (var i = 0; i < parameters.Length; i++)
                {
                    pairArray.Add(new KeyValuePair<string, string>(Parameters[i], parameters[i]));
                }
                var response = client.PostAsync(newUrl, new FormUrlEncodedContent(pairArray.ToArray())).Result;

                if (immediateCallback != null)
                    immediateCallback(response);
                if (OnResponse != null)
                    OnResponse(response);

                response.Dispose();
            }
        }
    }
}
