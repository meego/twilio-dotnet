using Moq;
using NUnit.Framework;
using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Api.Tests;
using Twilio.Lookups;

namespace Twilio.Api.Lookups.Net35.Tests
{
    [TestFixture]
    public class PhoneNumberTests
    {
        private const string ACCOUNT_SID = "AC123";

        private const string PHONE_NUMBER = "+1234567890";
        private const string COUNTRY_CODE = "US";

        ManualResetEvent manualResetEvent = null;

        private Mock<TwilioLookupsClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioLookupsClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetPhoneNumber()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Number>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Number());
            var client = mockClient.Object;

            client.GetPhoneNumber(PHONE_NUMBER);

            mockClient.Verify(trc => trc.Execute<Number>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/{PhoneNumber}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);

            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var phoneNumberParam = savedRequest.Parameters.Find(x => x.Name == "PhoneNumber");
            Assert.IsNotNull(phoneNumberParam);
            Assert.AreEqual(PHONE_NUMBER, phoneNumberParam.Value);
        }

        [Test]
        public void ShouldGetPhoneNumberAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()))
                .Callback<RestRequest, Action<Number>>((request, action) => savedRequest = request);
            var client = mockClient.Object;

            manualResetEvent = new ManualResetEvent(false);
            client.GetPhoneNumber(PHONE_NUMBER, number =>
            {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()), Times.Once);

            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("PhoneNumbers/{PhoneNumber}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);

            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var phoneNumberParam = savedRequest.Parameters.Find(x => x.Name == "PhoneNumber");
            Assert.IsNotNull(phoneNumberParam);
            Assert.AreEqual(PHONE_NUMBER, phoneNumberParam.Value);
        }

        [Test]
        public void ShouldGetPhoneNumberFromCountry()
        {
                
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Number>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Number());
            var client = mockClient.Object;

            client.GetPhoneNumber(PHONE_NUMBER, COUNTRY_CODE);

            mockClient.Verify(trc => trc.Execute<Number>(It.IsAny<RestRequest>()), Times.Once);

            Assert.IsNotNull(savedRequest);

            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var countryCodeParam = savedRequest.Parameters.Find(x => x.Name == "country_code");
            Assert.IsNotNull(countryCodeParam);
            Assert.AreEqual(COUNTRY_CODE, countryCodeParam.Value);
        }

        [Test]
        public void ShouldGetPhoneNumberFromCountryAsynchronously()
        {

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()))
                .Callback<RestRequest, Action<Number>>((request, action) => savedRequest = request);
            var client = mockClient.Object;

            manualResetEvent = new ManualResetEvent(false);
            client.GetPhoneNumber(PHONE_NUMBER, COUNTRY_CODE, number =>
            {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()), Times.Once);

            Assert.IsNotNull(savedRequest);

            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var countryCodeParam = savedRequest.Parameters.Find(x => x.Name == "country_code");
            Assert.IsNotNull(countryCodeParam);
            Assert.AreEqual(COUNTRY_CODE, countryCodeParam.Value);
        }
        
        [Test]
        public void ShouldGetPhoneNumberWithCarrierInfo()
        {

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Number>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Number());
            var client = mockClient.Object;

            client.GetPhoneNumber(PHONE_NUMBER, COUNTRY_CODE, true);

            mockClient.Verify(trc => trc.Execute<Number>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);

            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var typeParam = savedRequest.Parameters.Find(x => x.Name == "type");
            Assert.IsNotNull(typeParam);
            Assert.AreEqual("carrier", typeParam.Value);
        }

        [Test]
        public void ShouldGetPhoneNumberWithCarrierInfoAsynchronously()
        {

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()))
                .Callback<RestRequest, Action<Number>>((request, action) => savedRequest = request);
            var client = mockClient.Object;

            manualResetEvent = new ManualResetEvent(false);
            client.GetPhoneNumber(PHONE_NUMBER, COUNTRY_CODE, true, number =>
            {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Number>(It.IsAny<RestRequest>(), It.IsAny<Action<Number>>()), Times.Once);

            Assert.IsNotNull(savedRequest);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var typeParam = savedRequest.Parameters.Find(x => x.Name == "type");
            Assert.IsNotNull(typeParam);
            Assert.AreEqual("carrier", typeParam.Value);
        }
    }
}
