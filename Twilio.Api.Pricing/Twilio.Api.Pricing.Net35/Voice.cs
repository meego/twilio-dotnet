using System;
using Simple;

namespace Twilio.Pricing
{
    public partial class PricingClient
    {
        public virtual VoiceCountryResult ListVoiceCountries()
        {
            var request = new RestRequest("GET");
            request.Resource = "Voice/Countries";

            return Execute<VoiceCountryResult>(request);
        }

        public virtual VoiceCountry GetVoiceCountry(string isoCountry)
        {
            var request = new RestRequest("GET");
            request.Resource = "Voice/Countries/{IsoCountry}";
            request.AddUrlSegment("IsoCountry", isoCountry);

            return Execute<VoiceCountry>(request);
        }

        public virtual VoiceNumber GetVoiceNumber(string number)
        {
            var request = new RestRequest("GET");
            request.Resource = "Voice/Numbers/{Number}";
            request.AddUrlSegment("Number", number);

            return Execute<VoiceNumber>(request);
        }
    }
}