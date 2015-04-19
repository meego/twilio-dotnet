using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twilio.Lookups
{
    public partial class LookupsClient
    {
        public virtual async Task<Number> GetPhoneNumberAsync(string phoneNumber)
        {
            return await GetPhoneNumberAsync(phoneNumber, false);
        }

        public virtual async Task<Number> GetPhoneNumberAsync(string phoneNumber, bool includeCarrierInfo)
        {
            return await GetPhoneNumberAsync(phoneNumber, String.Empty, includeCarrierInfo);
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
