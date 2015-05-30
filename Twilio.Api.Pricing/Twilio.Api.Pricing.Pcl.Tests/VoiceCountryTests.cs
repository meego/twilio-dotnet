using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Simple;
using Twilio.Api.Tests;
using System.Threading.Tasks;

namespace Twilio.Pricing.Tests
{
    [TestFixture]
    public class VoiceCountryTests
    {
        private Mock<PricingClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<PricingClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetVoiceCountry()
        {
            var tcs = new TaskCompletionSource<VoiceCountry>();
            tcs.SetResult(new VoiceCountry());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<VoiceCountry>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetVoiceCountryAsync("AC");

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
        public async Task ShouldListVoiceCountries()
        {
            var tcs = new TaskCompletionSource<VoiceCountryResult>();
            tcs.SetResult(new VoiceCountryResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<VoiceCountryResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListVoiceCountriesAsync();

            mockClient.Verify(trc => trc.Execute<VoiceCountryResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Voice/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
    }
}
