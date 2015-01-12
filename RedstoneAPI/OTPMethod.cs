using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedstoneAPI
{
    public class OTPMethod
    {
        public string Type
        {
            get;
            private set;
        }
        public string DestinationId
        {
            get;
            private set;
        }
        public string DestinationDescription
        {
            get;
            private set;
        }

        public OTPMethod(string onClickMethod)
        {
            ParseOnClickString(onClickMethod);
        }

        public void ParseOnClickString(string data)
        {
            List<string> Params = new List<string>();

            while (data.Contains("'"))
            {
                var startParam = data.IndexOf("'") + 1;

                Params.Add(data.Substring(startParam, data.IndexOf("'", startParam) - startParam));
                data = data.Remove(0, data.IndexOf("'", startParam) + 1);
            }

            DestinationId = Params[0];
            Type = Params[1];
            DestinationDescription = Params[2];
        }

        public override string ToString()
        {
            return "Destination: " + DestinationDescription + "; DestinationId: " + DestinationId + "; Type: " + Type;
        }
    }
}
