using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public partial class TwilioLookupsClient
    {
        public virtual Number GetPhoneNumber(string phoneNumber)
        {
            return GetPhoneNumber(phoneNumber, String.Empty);
        }

        public virtual Number GetPhoneNumber(string phoneNumber, string countryCode)
        {
            return GetPhoneNumber(phoneNumber, countryCode, false);
        }

        public virtual Number GetPhoneNumber(string phoneNumber, string countryCode, bool includeCarrierInfo)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
                request.AddParameter("CountryCode", countryCode);

            if (includeCarrierInfo)
                request.AddParameter("Type", "carrier");

            return Execute<Number>(request);
        }

    }
}
