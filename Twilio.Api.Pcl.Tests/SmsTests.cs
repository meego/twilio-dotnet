using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class SmsTests
    {
        private const string FROM = "+15005550006";

        private const string TO = "+13144586142";

        ManualResetEvent manualResetEvent = null;

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldSendSmsMessage()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<SMSMessage>();
            tcs.SetResult(new SMSMessage());

            mockClient.Setup(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var body = ".NET Unit Test Message";
            await client.SendSmsMessage(FROM, TO, body);

            mockClient.Verify(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var fromParam = savedRequest.Parameters.Find(x => x.Name == "From");
            Assert.IsNotNull(fromParam);
            Assert.AreEqual(FROM, fromParam.Value);
            var toParam = savedRequest.Parameters.Find(x => x.Name == "To");
            Assert.IsNotNull(toParam);
            Assert.AreEqual(TO, toParam.Value);
            var bodyParam = savedRequest.Parameters.Find(x => x.Name == "Body");
            Assert.IsNotNull(bodyParam);
            Assert.AreEqual(body, bodyParam.Value);
        }


        [Test]
        public async Task ShouldSendSmsMessageWithUnicodeCharacters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<SMSMessage>();
            tcs.SetResult(new SMSMessage());

            mockClient.Setup(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var body = "رسالة اختبار وحدة.NET";
            await client.SendSmsMessage(FROM, TO, body);

            mockClient.Verify(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var fromParam = savedRequest.Parameters.Find(x => x.Name == "From");
            Assert.IsNotNull(fromParam);
            Assert.AreEqual(FROM, fromParam.Value);
            var toParam = savedRequest.Parameters.Find(x => x.Name == "To");
            Assert.IsNotNull(toParam);
            Assert.AreEqual(TO, toParam.Value);
            var bodyParam = savedRequest.Parameters.Find(x => x.Name == "Body");
            Assert.IsNotNull(bodyParam);
            Assert.AreEqual(body, bodyParam.Value);
        }

        [Test]
        public async Task ShouldListSmsMessages()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<SmsMessageResult>();
            tcs.SetResult(new SmsMessageResult());
            
            mockClient.Setup(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListSmsMessages();

            mockClient.Verify(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListSmsMessagesWithFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<SmsMessageResult>();
            tcs.SetResult(new SmsMessageResult());
            
            mockClient.Setup(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListSmsMessages(TO, FROM, null, null, null);

            mockClient.Verify(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var fromParam = savedRequest.Parameters.Find(x => x.Name == "From");
            Assert.IsNotNull(fromParam);
            Assert.AreEqual(FROM, fromParam.Value);
            var toParam = savedRequest.Parameters.Find(x => x.Name == "To");
            Assert.IsNotNull(toParam);
            Assert.AreEqual(TO, toParam.Value);
        }
    }
}