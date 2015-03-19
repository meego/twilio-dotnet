using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    /// <summary>
    /// REST API wrapper.
    /// </summary>
    public partial class TwilioLookupsClient : TwilioRestClient
    {
                /// <summary>
        /// Initializes a new client with the specified credentials.
        /// </summary>
        /// <param name="accountSid">The AccountSid to authenticate with</param>
        /// <param name="authToken">The AuthToken to authenticate with</param>
        public TwilioLookupsClient(string accountSid, string authToken) : this(accountSid, authToken, accountSid) { }

        /// <summary>
        /// Initializes a new client with the specified credentials.
        /// </summary>
        /// <param name="accountSid">The AccountSid to authenticate with</param>
        /// <param name="authToken">The AuthToken to authenticate with</param>
        /// <param name="accountResourceSid"></param>
        public TwilioLookupsClient(string accountSid, string authToken, string accountResourceSid) : base(accountSid, authToken, accountResourceSid, "v1", "https://taskrouter.twilio.com/") { }

    }
}
