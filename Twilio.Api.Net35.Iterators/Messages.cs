//using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple;

namespace Twilio
{
    public static class TwilioRestClientExtensions
    {
        public static IEnumerable<MessageResult> ListAllMessages(this TwilioRestClient client, MessageListRequest options)
        {
            Uri nextPageUri = null;

            while (true)
            {
                MessageResult result = null;

                if (nextPageUri == null)
                {
                    var request = new RestRequest();
                    request.Resource = "Accounts/{AccountSid}/Messages.json";
                    client.AddMessageListOptions(options, request);
                    result = client.Execute<MessageResult>(request);
                }
                else
                {
                    var request = new RestRequest();
                    request.Resource = nextPageUri.OriginalString.Replace("/" + client.ApiVersion, "");
                    result = client.Execute<MessageResult>(request);
                }

                if (result.NextPageUri != null)
                {
                    nextPageUri = result.NextPageUri;
                    yield return result;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
