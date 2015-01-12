using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;


namespace RedstoneAPI
{
    public class Session
    {
        public enum SessionStates
        {
            NotLoggedIn,
            WaitingForIdentity,
            LoggedIn,
            ErrorOccured,
        }

        public HttpClient Client;
        public HttpClientHandler Handler;

        public OTPMethod[] OTPMethods
        {
            get;private set;
        }

        public string CSRFTToken
        {
            get;
            set;
        }

        public string RFToken
        {
            get;
            set;
        }

        public SessionStates State
        {
            get;
            set;
        }

        public Session()
        {
            State = SessionStates.NotLoggedIn;

            CSRFTToken = "";
            RFToken = "";
            OTPMethods = null;

            Handler = new HttpClientHandler();
            Handler.UseCookies = true;
            Handler.CookieContainer = new CookieContainer();
            Handler.AllowAutoRedirect = true;
            Handler.MaxAutomaticRedirections = 100;
            
            
            SetMAF("NONE");
            Client = new HttpClient(Handler);
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            Client.BaseAddress = new Uri("https://www.redfcuonline.org");
            Client.Timeout = new TimeSpan(0, 0, 0, 10, 0);
        }

        public void Login(string member, string pass)
        {
            Endpoints.InitialLogin.Send(new string[] { member, pass, "11", "15" }, Client, (HttpResponseMessage response) =>
            {
                var resultString = response.Content.ReadAsStringAsync().Result;

                //Check if the login is fully completed
                if (postLogin(resultString))
                    return;
                if (!attemptOTPScrape(resultString))
                {
                    if (checkForErrors(resultString))
                    {
                        State = SessionStates.ErrorOccured;
                        throw new Exception("Login error occured");
                    }
                    else
                    {
                        State = SessionStates.ErrorOccured;
                        throw new Exception("Couldn't scrape CSRFToken");
                    }
                }
                else
                {
                    State = SessionStates.WaitingForIdentity;
                }
            });
        }

        public string GetMAF()
        {
            return Handler.CookieContainer.GetCookies(new Uri("https://www.redfcuonline.org"))["MAF_IB_1276015670"].Value;
        }

        public void SetMAF(string value)
        {
            Handler.CookieContainer.Add(new Uri("https://www.redfcuonline.org"), new Cookie("MAF_IB_1276015670", value, "/"));
        }

        public void Logout()
        {
            Endpoints.Logout.Send(new string[] {  }, Client);
        }

        public void RequestConfirmationCode(OTPMethod method)
        {
            Endpoints.RequestIdentityConfirmation.Send(new string[] { method.Type, method.DestinationId, CSRFTToken }, Client);
        }

        public void SendIdentityCode(string code, OTPMethod method)
        {
            Endpoints.SendIdentityCode.Send(new string[] { code, "OTP", "true", method.DestinationId, method.Type, CSRFTToken }, Client, (HttpResponseMessage response) =>
            {
                if (postLogin(response.Content.ReadAsStringAsync().Result) == false)
                {
                    State = SessionStates.ErrorOccured;
                    throw new Exception("No SESSION_TOKEN found");
                }
            });
        }

        public Cookie[] GetCookies()
        {
            List<Cookie> retList = new List<Cookie>();
            
            var cookies = Handler.CookieContainer.GetCookies(new Uri("https://www.redfcuonline.org/tob/live/usp-core"));
            foreach(Cookie cookie in cookies)
            {
                retList.Add(cookie);
            }
            cookies = Handler.CookieContainer.GetCookies(new Uri("https://www.redfcuonline.org"));
            foreach (Cookie cookie in cookies)
            {
                retList.Add(cookie);
            }
            return retList.ToArray();
        }

        private bool attemptOTPScrape(string resultString)
        {
            if (resultString.Contains("CSRFToken=") == false)
            {
                return false;
            }

            var startOfToken = resultString.IndexOf("CSRFToken=") + ("CSRFToken=").Length;
            CSRFTToken = resultString.Substring(startOfToken, resultString.IndexOf("';", startOfToken) - startOfToken);

            List<String> otps = new List<string>();
            while(resultString.Contains("OTP_Component.sendOTP("))
            {
                var startParam = resultString.IndexOf("OTP_Component.sendOTP(") + 1;

                otps.Add(resultString.Substring(startParam, resultString.IndexOf("\"", startParam) - startParam));
                resultString = resultString.Remove(0, resultString.IndexOf("\"", startParam) + 1);
            }

            if (otps.Count == 0)
            {
                return false;
            }

            OTPMethods = new OTPMethod[otps.Count];
            for (var i = 0; i < otps.Count; i++)
            {
                OTPMethods[i] = new OTPMethod(otps[i]);
            }
            return true;
        }

        private bool checkForErrors(string resultString)
        {
            return resultString.Contains("<div id=\"errors\">");
        }

        /// <summary>
        /// Grabs latest tokens to perform actions related to the current session
        /// </summary>
        private bool postLogin(string resultString)
        {
            var cookies = Handler.CookieContainer.GetCookies(new Uri("https://www.redfcuonline.org"));
            if (cookies["SESSION_TOKEN"] == null)
            {
                return false;
            }

            Endpoints.GetSDP.Send(new string[] { "true" }, Client, (HttpResponseMessage response) =>
            {
                resultString = response.Content.ReadAsStringAsync().Result;

                if (resultString.Contains("rftoken:") == false)
                {
                    State = SessionStates.ErrorOccured;
                    throw new Exception("Could not scrape rftoken");
                }
                else
                {
                    var startToken = resultString.IndexOf("rftoken: \"") + ("rftoken: \"").Length;
                    RFToken = resultString.Substring(startToken, resultString.IndexOf("\"", startToken) - startToken);
                }

                State = SessionStates.LoggedIn;
            });

            Endpoints.GetHome.Send(new string[] {}, Client, (HttpResponseMessage response) =>
            {
                resultString = response.Content.ReadAsStringAsync().Result;

                if (resultString.Contains("CSRFToken=") == false)
                {
                    State = SessionStates.ErrorOccured;
                    throw new Exception("Could not scrape CSRFToken");
                }
                else
                {
                    var startToken = resultString.IndexOf("CSRFToken=") + ("CSRFToken=").Length;
                    CSRFTToken = resultString.Substring(startToken, resultString.IndexOf("'", startToken) - startToken);
                }

                State = SessionStates.LoggedIn;
            });
            return true;
        }
    }
}
