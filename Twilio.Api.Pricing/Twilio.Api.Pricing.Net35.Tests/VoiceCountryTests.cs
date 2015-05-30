using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Simple;

using Twilio.Api.Tests;

namespace Twilio.Pricing.Tests
{
    [TestFixture]
    public class VoiceCountryTests
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
            mockClient.Setup(trc => trc.Execute<VoiceCountry>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new VoiceCountry());
            var client = mockClient.Object;

            client.GetVoiceCountry("AC");

            mockClient.Verify(trc => trc.Execute<VoiceCountry>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Countries/{IsoCountry}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("AC", isoCountryParam.Value);
        }

        [Test]
        public void ShouldGetVoiceCountryAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<VoiceCountry>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceCountry>>()))
                .Callback<RestRequest, Action<VoiceCountry>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetVoiceCountry("AC", app => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<VoiceCountry>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceCountry>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Countries/{IsoCountry}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("AC", isoCountryParam.Value);
        }

        [Test]
        public void ShouldListVoiceCountries()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<VoiceCountryResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new VoiceCountryResult());
            var client = mockClient.Object;

            client.ListVoiceCountries();

            mockClient.Verify(trc => trc.Execute<VoiceCountryResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldListVoiceCountriesAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<VoiceCountryResult>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceCountryResult>>()))
                .Callback<RestRequest, Action<VoiceCountryResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListVoiceCountries(workspaces => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<VoiceCountryResult>(It.IsAny<RestRequest>(), It.IsAny<Action<VoiceCountryResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
    }
}
