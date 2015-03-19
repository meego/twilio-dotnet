using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twilio.Lookups
{
    public partial class TwilioLookupsClient
    {
        public virtual async Task<PhoneNumber> GetPhoneNumber(string phoneNumber)
        {
            return await GetPhoneNumber(phoneNumber, String.Empty);
        }

        public virtual async Task<PhoneNumber> GetPhoneNumber(string phoneNumber, string countryCode)
        {
            return await GetPhoneNumber(phoneNumber, countryCode, false);
        }

        public virtual async Task<PhoneNumber> GetPhoneNumber(string phoneNumber, string countryCode, bool includeCarrierInfo)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
                request.AddParameter("country_code", countryCode);

            if (includeCarrierInfo)
                request.AddParameter("type", "carrier");

            return await Execute<PhoneNumber>(request);
        }

    }
}
