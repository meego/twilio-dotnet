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
    public class VoiceNumberTests
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
            var tcs = new TaskCompletionSource<VoiceNumber>();
            tcs.SetResult(new VoiceNumber());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<VoiceNumber>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetVoiceNumberAsync("+14155551234");

            mockClient.Verify(trc => trc.Execute<VoiceNumber>(It.IsAny<RestRequest>()), Times.Once);
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
