using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public partial class TwilioLookupsClient
    {
        public virtual PhoneNumber GetPhoneNumber(string phoneNumber)
        {
            return GetPhoneNumber(phoneNumber, String.Empty);
        }

        public virtual PhoneNumber GetPhoneNumber(string phoneNumber, string countryCode)
        {
            return GetPhoneNumber(phoneNumber, countryCode, false);
        }

        public virtual PhoneNumber GetPhoneNumber(string phoneNumber, string countryCode, bool includeCarrierInfo)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
                request.AddParameter("country_code", countryCode);

            if (includeCarrierInfo)
                request.AddParameter("type", "carrier");

            return Execute<PhoneNumber>(request);
        }

    }
}
