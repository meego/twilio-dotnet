using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Simple;

using Twilio.Api.Tests;

namespace Twilio.Pricing.Tests
{
    [TestFixture]
    public class PhoneNumberCountryTests
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
        public void ShouldGetPhoneNumberCountry()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<PhoneNumberCountry>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new PhoneNumberCountry());
            var client = mockClient.Object;

            client.GetPhoneNumberCountry("AC");

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
        public void ShouldGetPhoneNumberCountryAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<PhoneNumberCountry>(It.IsAny<RestRequest>(), It.IsAny<Action<PhoneNumberCountry>>()))
                .Callback<RestRequest, Action<PhoneNumberCountry>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetPhoneNumberCountry("AC", app => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<PhoneNumberCountry>(It.IsAny<RestRequest>(), It.IsAny<Action<PhoneNumberCountry>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/Countries/{IsoCountry}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("AC", isoCountryParam.Value);
        }

        [Test]
        public void ShouldListPhoneNumberCountries()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<PhoneNumberCountryResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new PhoneNumberCountryResult());
            var client = mockClient.Object;

            client.ListPhoneNumberCountries();

            mockClient.Verify(trc => trc.Execute<PhoneNumberCountryResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldListPhoneNumberCountriesAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<PhoneNumberCountryResult>(It.IsAny<RestRequest>(), It.IsAny<Action<PhoneNumberCountryResult>>()))
                .Callback<RestRequest, Action<PhoneNumberCountryResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListPhoneNumberCountries(workspaces => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<PhoneNumberCountryResult>(It.IsAny<RestRequest>(), It.IsAny<Action<PhoneNumberCountryResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/Countries", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
    }
}
