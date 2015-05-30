using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Pricing
{
    public partial class PricingClient
    {
        public virtual async Task<VoiceCountryResult> ListVoiceCountriesAsync()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "Voice/Countries";

            return await Execute<VoiceCountryResult>(request);
        }

        public virtual async Task<VoiceCountry> GetVoiceCountryAsync(string isoCountry)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "Voice/Countries/{IsoCountry}";
            request.AddUrlSegment("IsoCountry", isoCountry);

            return await Execute<VoiceCountry>(request);
        }

        public virtual async Task<VoiceNumber> GetVoiceNumberAsync(string number)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "Voice/Numbers/{Number}";
            request.AddUrlSegment("Number", number);

            return await Execute<VoiceNumber>(request);
        }
    }
}