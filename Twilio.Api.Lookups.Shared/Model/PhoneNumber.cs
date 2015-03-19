using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public class PhoneNumber
    {
        public string Number { get; set; }

        public string NationalFormat { get; set; }

        public string Countryode { get; set; }

        public Carrier Carrier { get; set; }
    }
}
