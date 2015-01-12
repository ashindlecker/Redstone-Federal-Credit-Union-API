using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RedstoneAPI;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Xml;

namespace RedstoneAPITests
{
    class Program
    {
        static Session session;

        static void Main(string[] args)
        {
            Console.WriteLine("Redstone Federal Credit Union API Console Test");
            Console.Write("Username / MemberID: ");
            var user = Console.ReadLine();
            Console.Write("Password: ");
            var pass = Console.ReadLine();

            session = new Session();
            try
            {
                session.Login(user, pass);
            }
            catch
            {
                Console.WriteLine("Login incorrect");
                return;
            }


            if(session.State != Session.SessionStates.LoggedIn)
            {
                Console.WriteLine("Need a confirmation code");
                Console.WriteLine("Sending code to " + session.OTPMethods[2].DestinationDescription);
                session.RequestConfirmationCode(session.OTPMethods[2]);

                confirmCode:
                Console.Write("Confirmation code: ");
                try
                {
                    session.SendIdentityCode(Console.ReadLine(), session.OTPMethods[2]);
                    if (session.State == Session.SessionStates.LoggedIn)
                    {
                        postLogin();
                    }
                }
                catch
                {
                    Console.WriteLine("Confirmation code incorrect, please retry");
                    goto confirmCode;
                }
            }
            else
            {
                postLogin();
            }
        }

        static void postLogin()
        {
            Console.WriteLine("Logged in...");
            var accounts = Account.GetAccounts(session);
            for (var i = 0; i < accounts.Length; i++)
                Console.WriteLine(accounts[i].AccountType + ": " + accounts[i].Balance);
        }

    }
}
