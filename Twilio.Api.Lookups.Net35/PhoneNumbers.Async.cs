using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public partial class LookupsClient
    {
        public virtual void GetPhoneNumber(string phoneNumber, Action<Number> callback)
        {
            GetPhoneNumber(phoneNumber, false, callback);
        }

        public virtual void GetPhoneNumber(string phoneNumber, bool includeCarrierInfo, Action<Number> callback)
        {
            GetPhoneNumber(phoneNumber, String.Empty, includeCarrierInfo, callback);
        }

        public virtual void GetPhoneNumber(string phoneNumber, string countryCode, bool includeCarrierInfo, Action<Number> callback)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
                request.AddParameter("CountryCode", countryCode);

            if (includeCarrierInfo)
                request.AddParameter("Type", "carrier");

            ExecuteAsync<Number>(request, (response) => { callback(response); });
        }

    }
}
