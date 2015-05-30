using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Simple;

using Twilio.Api.Tests;

namespace Twilio.Pricing.Tests
{
    [TestFixture]
    public class VoiceNumberTests
    {
        ManualResetEvent manualResetEvent = null;

        private Mock<PricingClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<PricingClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetVoiceCountry()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<VoiceNumber>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new VoiceNumber());
            var client = mockClient.Object;

            client.GetVoiceNumber("+14155551234");

            mockClient.Verify(trc => trc.Execute<VoiceNumber>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Numbers/{Number}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var numberParam = savedRequest.Parameters.Find(x => x.Name == "Number");
            Assert.IsNotNull(numberParam);
            Assert.AreEqual("+14155551234", numberParam.Value);
        }

        [Test]
        public void ShouldGetVoiceCountryAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<VoiceNumber>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceNumber>>()))
                .Callback<RestRequest, Action<VoiceNumber>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetVoiceNumber("+14155551234", app => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<VoiceNumber>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceNumber>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Numbers/{Number}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var numberParam = savedRequest.Parameters.Find(x => x.Name == "Number");
            Assert.IsNotNull(numberParam);
            Assert.AreEqual("+14155551234", numberParam.Value);
        }
    }
}
