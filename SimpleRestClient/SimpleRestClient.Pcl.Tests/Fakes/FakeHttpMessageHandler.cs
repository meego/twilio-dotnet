using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRestClient.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        HttpResponseMessage response;
        Exception exception;

        public FakeHttpMessageHandler(HttpStatusCode statusCode) {
            response = new HttpResponseMessage(statusCode);
        }

        public FakeHttpMessageHandler(Exception exc)
        {
            exception = exc;
        }

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content)
            };
        }

#pragma warning disable 1998
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (exception != null)
                throw exception;

            return response;
        }
#pragma warning restore 1998
    }
}
