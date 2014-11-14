using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace SimpleRestClient.Tests
{

    [TestFixture]
    public class RestResponseTests
    {
        RestClient client;
        string BASE_URL = "http://twilio.com";

        [SetUp]
        public void Setup()
        {
            client = new RestClient();
            client.BaseUrl = BASE_URL;
        }

        [Test]
        public async Task When_Http_Protocol_Error_Then_Response_Contains_Status_Code_And_Description()
        {
            client.MessageHandler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest);

            var request = new RestRequest();
            var restresponse = await client.ExecuteAsync<RestRequest>(request);           

            Assert.AreEqual(HttpStatusCode.BadRequest, restresponse.StatusCode);
            Assert.AreEqual("BAD REQUEST", restresponse.StatusDescription);
            Assert.AreEqual(ResponseStatus.Completed, restresponse.ResponseStatus);
        }

        [Test]
        public async Task When_Http_Protocol_Error_Then_Response_Contains_Content()
        {
            var sourcecontent = "{\"code\": 90011, \"message\": \"Param From must be specified.\", \"more_info\": \"https://www.twilio.com/docs/errors/90011\", \"status\": 400}";

            client.MessageHandler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, sourcecontent);

            var request = new RestRequest();
            var restresponse = await client.ExecuteAsync<RestRequest>(request);           

            Assert.AreEqual(HttpStatusCode.BadRequest, restresponse.StatusCode);
            Assert.AreEqual((int)sourcecontent.Length, restresponse.RawBytes.Length);
            CollectionAssert.AreEquivalent(sourcecontent, restresponse.RawBytes);
            Assert.AreEqual(ResponseStatus.Completed, restresponse.ResponseStatus);
        }

        [Test]
        public async Task When_Http_Request_Times_Out_Then_Populate_Exception_Properties()
        {
            var message = "The operation has timed out";

            client.MessageHandler = new FakeHttpMessageHandler(new TaskCanceledException());

            var request = new RestRequest();
            var restresponse = await client.ExecuteAsync<RestRequest>(request);

            Assert.IsNotNull(restresponse.ErrorException);
            Assert.AreEqual(message, restresponse.ErrorMessage);
            Assert.AreEqual(ResponseStatus.Error, restresponse.ResponseStatus);
        }

        [Test]
        public async Task When_Http_Request_Is_Canceled_Then_Populate_Exception_Properties()
        {
            var message = "The operation has timed out";

            client.MessageHandler = new FakeHttpMessageHandler(new TaskCanceledException());

            var request = new RestRequest();

            var mCancellationTokenSource = new CancellationTokenSource();
            var restresponse = await client.ExecuteAsync(request, mCancellationTokenSource.Token);
            mCancellationTokenSource.Cancel();

            Assert.IsNotNull(restresponse.ErrorException);
            Assert.AreEqual(message, restresponse.ErrorMessage);
            Assert.AreEqual(ResponseStatus.Error, restresponse.ResponseStatus);
        }

        [Test]
        public async Task When_Http_Request_Completes_Successfully_Then_Extract_Response()
        {
            var sourcecontent = "{\"sid\": \"SMb2628b9fb5992e2f117891601451084b\", \"date_created\": \"Thu, 03 Apr 2014 02:11:55 +0000\", \"date_updated\": \"Thu, 03 Apr 2014 02:11:58 +0000\", \"date_sent\": \"Thu, 03 Apr 2014 02:11:58 +0000\", \"account_sid\": \"AC3137d76457814a5eabf7de62f346d39a\", \"to\": \"+13144586142\", \"from\": \"+19108638087\", \"body\": \"Enter '1234' to confirm your identity and access your account.\", \"status\": \"delivered\", \"num_segments\": \"1\", \"num_media\": null, \"direction\": \"outbound-api\", \"api_version\": \"2010-04-01\", \"price\": \"-0.00750\", \"price_unit\": \"USD\", \"uri\": \"/2010-04-01/Accounts/AC3137d76457814a5eabf7de62f346d39a/Messages/SMb2628b9fb5992e2f117891601451084b.json\", \"subresource_uris\": {\"media\": \"/2010-04-01/Accounts/AC3137d76457814a5eabf7de62f346d39a/Messages/SMb2628b9fb5992e2f117891601451084b/Media.json\"}}";
            client.MessageHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, sourcecontent);

            var request = new RestRequest();
            var restresponse = await client.ExecuteAsync<RestRequest>(request);           

            Assert.AreEqual(HttpStatusCode.OK, restresponse.StatusCode);
            Assert.AreEqual((int)sourcecontent.Length, restresponse.RawBytes.Length);
            CollectionAssert.AreEquivalent(sourcecontent, restresponse.RawBytes);
        }
    }
}
