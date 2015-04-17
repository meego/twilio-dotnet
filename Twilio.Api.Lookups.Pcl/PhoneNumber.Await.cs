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
        public virtual async Task<Number> GetPhoneNumberAsync(string phoneNumber)
        {
            return await GetPhoneNumberAsync(phoneNumber, String.Empty);
        }

        public virtual async Task<Number> GetPhoneNumberAsync(string phoneNumber, string countryCode)
        {
            return await GetPhoneNumberAsync(phoneNumber, countryCode, false);
        }

        public virtual async Task<Number> GetPhoneNumberAsync(string phoneNumber, string countryCode, bool includeCarrierInfo)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
            {
                request.AddParameter("CountryCode", countryCode);
            }
            if (includeCarrierInfo)
            {
                request.AddParameter("Type", "carrier");
            }

            return await Execute<Number>(request);
        }

    }
}
