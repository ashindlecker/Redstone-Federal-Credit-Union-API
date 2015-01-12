using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedstoneAPI
{
    //Endpoints for the Redstone Federal Credit Union webpage
    //Last updated January 6, 2015
    public class Endpoints
    {
        /// <summary>
        /// Used at the main page where you enter member name and password. Can either succeed instantly or require identiy confirmation
        /// POST
        /// userid - The member number or name
        /// password - the member password
        /// x - unknown, set to random number
        /// y - unknown, set to random number
        /// </summary>
        public static Endpoint InitialLogin = new Endpoint("/tob/live/usp-core/app/initialLogin", new string[] { "userid", "password", "x", "y" }, "POST");

        public static Endpoint RedirectInitialLogin = new Endpoint("/tob/live/usp-core/app/redirectInitialLogin", new string[] { "mfaLSO" }, "POST");
        /// <summary>
        /// Called to receive a confirmation text, email, or call with a code to enter
        /// POST
        /// type - The type of method to send the code (email)
        /// destId - The id of the type to send to
        /// CSRFToken - Token
        /// </summary>
        public static Endpoint RequestIdentityConfirmation = new Endpoint("/tob/live/usp-core/app/mfa", new string[] { "type", "destId", "CSRFToken"}, "POST");


        
        /// <summary>
        /// Called to send the confirmation code received in an email, text, or call
        /// POST
        /// type - The type of method to send the code (email)
        /// destId - The id of the type to send to
        /// CSRFToken - Token
        /// </summary>
        public static Endpoint SendIdentityCode = new Endpoint("/tob/live/usp-core/app/mfa", new string[] { "otp", "type", "cookieoptin", "destId", "displayMethod", "CSRFToken" }, "POST");

        /// <summary>
        /// Called to get rftoken
        /// GET
        /// usp - unknown, set to true
        /// </summary>
        public static Endpoint GetSDP = new Endpoint("/tob/live/usp-core/sdp/com.diginsite.product.sdp.SDP/SDP.htm", new string[] { "usp" }, "GET");

        /// <summary>
        /// Called to get home page (containes csrtftoken)
        /// GET
        /// </summary>
        public static Endpoint GetHome = new Endpoint("/tob/live/usp-core/app/home", new string[] {}, "GET");

        /// <summary>
        /// Called to get accounts information (balance, etc)
        /// GET
        /// rftoken - Token grabbed after successful login
        /// allowAccountsCall - Unknown, set to true
        /// </summary>
        public static Endpoint GetAccounts = new Endpoint("/tob/live/usp-core/sdp/app/ajax/history/accounts.json", new string[] { "rftoken", "allowAccountsCall" }, "GET");

        /// <summary>
        /// Called to get transaction history of an account
        /// GET
        /// rftoken - Token from successful login
        /// accountId - Id of the account to get history from
        /// dateRangeStart - start time of history
        /// dateRangeEnd - end time of history
        /// </summary>
        public static Endpoint GetTransactions = new Endpoint("/tob/live/usp-core/sdp/app/ajax/history/transactions.json", new string[] { "rftoken", "accountId", "dateRangeStart", "dateRangeEnd" }, "GET");

        /// <summary>
        /// Logout
        /// GET
        /// CSRFToken - Token
        /// </summary>
        public static Endpoint Logout = new Endpoint("/tob/live/usp-core/sdp/logout", new string[] {}, "POST");

    }
}
