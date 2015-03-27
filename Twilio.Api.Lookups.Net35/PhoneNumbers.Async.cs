using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public partial class TwilioLookupsClient
    {
        public virtual void GetPhoneNumber(string phoneNumber, Action<Number> callback)
        {
            GetPhoneNumber(phoneNumber, String.Empty, callback);
        }

        public virtual void GetPhoneNumber(string phoneNumber, string countryCode, Action<Number> callback)
        {
            GetPhoneNumber(phoneNumber, countryCode, false, callback);
        }

        public virtual void GetPhoneNumber(string phoneNumber, string countryCode, bool includeCarrierInfo, Action<Number> callback)
        {
            var request = new RestRequest();
            request.Resource = "PhoneNumbers/{PhoneNumber}";

            request.AddUrlSegment("PhoneNumber", phoneNumber);

            if (countryCode.HasValue())
                request.AddParameter("country_code", countryCode);

            if (includeCarrierInfo)
                request.AddParameter("type", "carrier");

            ExecuteAsync<Number>(request, (response) => { callback(response); });
        }

    }
}
