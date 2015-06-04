//using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Simple;
using System.Reactive.Disposables;
using System.Reactive;

namespace Twilio
{
    public static class TwilioRestClientExtensions
    {
        public static IObservable<MessageResult> ListAllMessagesAsync(this TwilioRestClient client, MessageListRequest options)
        {
            return System.Reactive.Linq.Observable.Create<MessageResult>(async (IObserver<MessageResult> observer) =>
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
                        result = await client.Execute<MessageResult>(request);
                    }
                    else
                    {
                        var request = new RestRequest();
                        request.Resource = nextPageUri.OriginalString.Replace("/" + client.ApiVersion, "");
                        result = await client.Execute<MessageResult>(request);
                    }

                    if (result.NextPageUri != null)
                    {
                        nextPageUri = result.NextPageUri;
                        observer.OnNext(result);
                    }
                    else
                    {
                        observer.OnCompleted();
                        break;
                    }

                }

                return () => { };
            });
        }
    }
}
