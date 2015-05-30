using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Pricing
{
    public partial class PricingClient
    {
        public virtual async Task<PhoneNumberCountryResult> ListPhoneNumberCountriesAsync()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "PhoneNumbers/Countries";

            return await Execute<PhoneNumberCountryResult>(request);
        }

        public virtual async Task<PhoneNumberCountry> GetPhoneNumberCountryAsync(string isoCountry)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "PhoneNumbers/Countries/{IsoCountry}";
            request.AddUrlSegment("IsoCountry", isoCountry);

            return await Execute<PhoneNumberCountry>(request);
        }
    }
}