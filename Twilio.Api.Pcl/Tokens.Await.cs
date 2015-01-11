using Simple;
using System.Threading.Tasks;
//using Simple.Validation;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="ttl">The friendly name to name the application</param>
        public virtual async Task<Token> CreateTokenAsync(int ttl)
        {
            var request = new RestRequest(Method.POST);
            //Require.Argument("Ttl", ttl);
            request.Resource = "Accounts/{AccountSid}/Tokens.json";
            request.AddParameter("Ttl", ttl);

            return await Execute<Token>(request);
        }

        /// <summary>
        /// Create a new token
        /// </summary>
        public virtual async Task<Token> CreateTokenAsync()
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Tokens.json";
            return await Execute<Token>(request);
        }
    }

}