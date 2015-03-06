using System;
using Simple;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Search available local phone numbers
        /// </summary>
        /// <param name="isoCountryCode">Two-character ISO country code (US or CA)</param>
        /// <param name="options">Search filter options. Only properties with values set will be used.</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListAvailableLocalPhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options, Action<AvailablePhoneNumberResult> callback)
        {
            Require.Argument("isoCountryCode", isoCountryCode);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Local.json";
            request.AddUrlSegment("IsoCountryCode", isoCountryCode);

            AddNumberSearchParameters(options, request);

            ExecuteAsync<AvailablePhoneNumberResult>(request, (response) => callback(response));
        }

        /// <summary>
        /// Search available toll-free phone numbers
        /// </summary>
        /// <param name="isoCountryCode">Two-character ISO country code (US or CA)</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListAvailableTollFreePhoneNumbers(string isoCountryCode, Action<AvailablePhoneNumberResult> callback)
        {
            ListAvailableTollFreePhoneNumbers(isoCountryCode, string.Empty, callback);
        }

        /// <summary>
        /// Search available toll-free phone numbers
        /// </summary>
        /// <param name="isoCountryCode">Two-character ISO country code (US or CA)</param>
        /// <param name="contains"></param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListAvailableTollFreePhoneNumbers(string isoCountryCode, string contains, Action<AvailablePhoneNumberResult> callback)
        {
            var options = new AvailablePhoneNumberListRequest() { Contains = contains };
            ListAvailableTollFreePhoneNumbers(isoCountryCode, options, callback);
        }

        /// <summary>
        /// Search available toll-free phone numbers
        /// </summary>
        /// <param name="isoCountryCode">Two-character ISO country code (US or CA)</param>
        /// <param name="options"></param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListAvailableTollFreePhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options, Action<AvailablePhoneNumberResult> callback)
        {
            Require.Argument("isoCountryCode", isoCountryCode);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/TollFree.json";
            request.AddUrlSegment("IsoCountryCode", isoCountryCode);

            AddNumberSearchParameters(options, request);

            ExecuteAsync<AvailablePhoneNumberResult>(request, (response) => callback(response));
        }

        /// <summary>
        /// Search available mobile phone numbers.  Makes a GET request to the AvailablePhoneNumber List resource.
        /// </summary>
        /// <param name="isoCountryCode">Two-character ISO country code (US or CA)</param>
        /// <param name="options">Search filter options. Only properties with values set will be used.</param>
        /// <param name="callback"></param>
        public virtual void ListAvailableMobilePhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options, Action<AvailablePhoneNumberResult> callback)
        {
            Require.Argument("isoCountryCode", isoCountryCode);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/AvailablePhoneNumbers/{IsoCountryCode}/Mobile.json";
            request.AddUrlSegment("IsoCountryCode", isoCountryCode);

            AddNumberSearchParameters(options, request);

            ExecuteAsync<AvailablePhoneNumberResult>(request, (response) => callback(response));
        }
    }
}
