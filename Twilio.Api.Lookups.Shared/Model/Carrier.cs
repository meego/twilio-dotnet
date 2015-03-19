using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public class Carrier
    {
        public string MobileCountryCode { get; set; }

        public string MobileNetworkCode { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ErrorCode { get; set; }
    }
}
