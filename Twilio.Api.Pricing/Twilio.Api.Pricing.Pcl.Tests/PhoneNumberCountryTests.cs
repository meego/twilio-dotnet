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
    public class PhoneNumberCountryTests
    {
        private Mock<PricingClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<PricingClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetPhoneNumberCountry()
        {
            var tcs = new TaskCompletionSource<PhoneNumberCountry>();
            tcs.SetResult(new PhoneNumberCountry());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<PhoneNumberCountry>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetPhoneNumberCountryAsync("AC");

            mockClient.Verify(trc => trc.Execute<PhoneNumberCountry>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/Countries/{IsoCountry}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("AC", isoCountryParam.Value);
        }

        [Test]
        public async Task ShouldListPhoneNumberCountries()
        {
            var tcs = new TaskCompletionSource<PhoneNumberCountryResult>();
            tcs.SetResult(new PhoneNumberCountryResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<PhoneNumberCountryResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListPhoneNumberCountriesAsync();

            mockClient.Verify(trc => trc.Execute<PhoneNumberCountryResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
    }
}
