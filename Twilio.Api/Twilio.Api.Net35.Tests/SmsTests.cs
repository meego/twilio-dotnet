﻿using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;

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
        public void ShouldSendSmsMessage()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new SMSMessage());
            var client = mockClient.Object;
            var body = ".NET Unit Test Message";

            client.SendSmsMessage(FROM, TO, body);

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
        public void ShouldSendSmsMessageAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<SMSMessage>(It.IsAny<RestRequest>(), It.IsAny<Action<SMSMessage>>()))
                .Callback<RestRequest, Action<SMSMessage>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var body = ".NET Unit Test Message";

            client.SendSmsMessage(FROM, TO, body, message =>
            {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<SMSMessage>(It.IsAny<RestRequest>(), It.IsAny<Action<SMSMessage>>()), Times.Once);
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
        public void ShouldSendSmsMessageWithUnicodeCharacters()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SMSMessage>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new SMSMessage());
            var client = mockClient.Object;
            var body = "رسالة اختبار وحدة.NET";

            client.SendSmsMessage(FROM, TO, body);

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
        public void ShouldListSmsMessages()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new SmsMessageResult());
            var client = mockClient.Object;

            client.ListSmsMessages();

            mockClient.Verify(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldListSmsMessagesAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<SmsMessageResult>(It.IsAny<RestRequest>(), It.IsAny<Action<SmsMessageResult>>()))
                .Callback<RestRequest, Action<SmsMessageResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListSmsMessages(messages =>
            {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<SmsMessageResult>(It.IsAny<RestRequest>(), It.IsAny<Action<SmsMessageResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/SMS/Messages.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldListSmsMessagesWithFilters()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<SmsMessageResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new SmsMessageResult());
            var client = mockClient.Object;

            client.ListSmsMessages(TO, FROM, null, null, null);

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