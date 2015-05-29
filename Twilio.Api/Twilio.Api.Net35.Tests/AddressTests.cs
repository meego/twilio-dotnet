using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using System.Linq;
using Twilio.Api.Tests;
using Simple;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class AddressTests
    {
        private const string ADDRESS_SID = "AD123";

        ManualResetEvent manualResetEvent = null;

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldAddNewAddress()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Address());
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.AddAddress(friendlyName, "Homer Simpson", "742 Evergreen Terrace", "Springfield", "MO", "65801", "US");

            mockClient.Verify(trc => trc.Execute<Address>(It.IsAny<RestRequest>()), Times.Once);

            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var customerNameParam = savedRequest.Parameters.Find(x => x.Name == "CustomerName");
            Assert.IsNotNull(customerNameParam);
            Assert.AreEqual("Homer Simpson", customerNameParam.Value);
            var streetParam = savedRequest.Parameters.Find(x => x.Name == "Street");
            Assert.IsNotNull(streetParam);
            Assert.AreEqual("742 Evergreen Terrace", streetParam.Value);
            var cityParam = savedRequest.Parameters.Find(x => x.Name == "City");
            Assert.IsNotNull(cityParam);
            Assert.AreEqual("Springfield", cityParam.Value);
            var regionParam = savedRequest.Parameters.Find(x => x.Name == "Region");
            Assert.IsNotNull(regionParam);
            Assert.AreEqual("MO", regionParam.Value);
            var postalCodeParam = savedRequest.Parameters.Find(x => x.Name == "PostalCode");
            Assert.IsNotNull(postalCodeParam);
            Assert.AreEqual("65801", postalCodeParam.Value);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("US", isoCountryParam.Value);
        }

        [Test]
        public void ShouldAddNewAddressAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()))
                .Callback<RestRequest, Action<Address>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.AddAddress(friendlyName, "Homer Simpson", "742 Evergreen Terrace", "Springfield", "MO", "65801", "US", address =>
                {
                    manualResetEvent.Set();
                });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var customerNameParam = savedRequest.Parameters.Find(x => x.Name == "CustomerName");
            Assert.IsNotNull(customerNameParam);
            Assert.AreEqual("Homer Simpson", customerNameParam.Value);
            var streetParam = savedRequest.Parameters.Find(x => x.Name == "Street");
            Assert.IsNotNull(streetParam);
            Assert.AreEqual("742 Evergreen Terrace", streetParam.Value);
            var cityParam = savedRequest.Parameters.Find(x => x.Name == "City");
            Assert.IsNotNull(cityParam);
            Assert.AreEqual("Springfield", cityParam.Value);
            var regionParam = savedRequest.Parameters.Find(x => x.Name == "Region");
            Assert.IsNotNull(regionParam);
            Assert.AreEqual("MO", regionParam.Value);
            var postalCodeParam = savedRequest.Parameters.Find(x => x.Name == "PostalCode");
            Assert.IsNotNull(postalCodeParam);
            Assert.AreEqual("65801", postalCodeParam.Value);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual("US", isoCountryParam.Value);
        }

        [Test]
        public void ShouldGetAddress()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Address());
            var client = mockClient.Object;

            client.GetAddress(ADDRESS_SID);

            mockClient.Verify(trc => trc.Execute<Address>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public void ShouldGetAddressAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()))
                .Callback<RestRequest, Action<Address>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetAddress(ADDRESS_SID, app => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public void ShouldListAddresses()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new AddressResult());
            var client = mockClient.Object;

            client.ListAddresses();

            mockClient.Verify(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public void ShouldListAddressesAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<AddressResult>(It.IsAny<RestRequest>(), It.IsAny<Action<AddressResult>>()))
                .Callback<RestRequest, Action<AddressResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListAddresses(addresses => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<AddressResult>(It.IsAny<RestRequest>(), It.IsAny<Action<AddressResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
        
        [Test]
        public void ShouldListAddressesUsingFilters()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new AddressResult());
            var client = mockClient.Object;
            var options = new AddressListRequest ();
            options.CustomerName = "Homer Simpson";
            options.FriendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            options.IsoCountry = "US";
            options.Page = 1;
            options.PageSize = 10;

            client.ListAddresses(options);

            mockClient.Verify(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            var customerNameParam = savedRequest.Parameters.Find(x => x.Name == "CustomerName");
            Assert.IsNotNull(customerNameParam);
            Assert.AreEqual(options.CustomerName, customerNameParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(options.FriendlyName, friendlyNameParam.Value);
            var isoCountryParam = savedRequest.Parameters.Find(x => x.Name == "IsoCountry");
            Assert.IsNotNull(isoCountryParam);
            Assert.AreEqual(options.IsoCountry, isoCountryParam.Value);
            var pageParam = savedRequest.Parameters.Find(x => x.Name == "Page");
            Assert.IsNotNull(pageParam);
            Assert.AreEqual(options.Page, pageParam.Value);
            var pageSizeParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(pageSizeParam);
            Assert.AreEqual(options.PageSize, pageSizeParam.Value);
        }

        [Test]
        public void ShouldUpdateAddress()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Address());
            var client = mockClient.Object;
            var options = new AddressOptions();
            options.City = "Springfield";
            options.CustomerName = "Homer Simpson";
            options.FriendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName ();
            options.PostalCode = "65801";
            options.Region = "MO";
            options.Street = "742 Evergreen Terrace";

            client.UpdateAddress(ADDRESS_SID, options);

            mockClient.Verify(trc => trc.Execute<Address>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
            var cityParam = savedRequest.Parameters.Find(x => x.Name == "City");
            Assert.IsNotNull(cityParam);
            Assert.AreEqual(options.City, cityParam.Value);
            var customerNameParam = savedRequest.Parameters.Find(x => x.Name == "CustomerName");
            Assert.IsNotNull(customerNameParam);
            Assert.AreEqual(options.CustomerName, customerNameParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(options.FriendlyName, friendlyNameParam.Value);
            var postalCodeParam = savedRequest.Parameters.Find(x => x.Name == "PostalCode");
            Assert.IsNotNull(postalCodeParam);
            Assert.AreEqual(options.PostalCode, postalCodeParam.Value);
            var regionParam = savedRequest.Parameters.Find(x => x.Name == "Region");
            Assert.IsNotNull(regionParam);
            Assert.AreEqual(options.Region, regionParam.Value);
            var streetParam = savedRequest.Parameters.Find(x => x.Name == "Street");
            Assert.IsNotNull(streetParam);
            Assert.AreEqual(options.Street, streetParam.Value);
        }

        [Test]
        public void ShouldUpdateAddressAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()))
                .Callback<RestRequest, Action<Address>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var options = new AddressOptions();
            options.City = "Springfield";
            options.CustomerName = "Homer Simpson";
            options.FriendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName ();
            options.PostalCode = "65801";
            options.Region = "MO";
            options.Street = "742 Evergreen Terrace";

            client.UpdateAddress(ADDRESS_SID, options, address => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Address>(It.IsAny<RestRequest>(), It.IsAny<Action<Address>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
            var cityParam = savedRequest.Parameters.Find(x => x.Name == "City");
            Assert.IsNotNull(cityParam);
            Assert.AreEqual(options.City, cityParam.Value);
            var customerNameParam = savedRequest.Parameters.Find(x => x.Name == "CustomerName");
            Assert.IsNotNull(customerNameParam);
            Assert.AreEqual(options.CustomerName, customerNameParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(options.FriendlyName, friendlyNameParam.Value);
            var postalCodeParam = savedRequest.Parameters.Find(x => x.Name == "PostalCode");
            Assert.IsNotNull(postalCodeParam);
            Assert.AreEqual(options.PostalCode, postalCodeParam.Value);
            var regionParam = savedRequest.Parameters.Find(x => x.Name == "Region");
            Assert.IsNotNull(regionParam);
            Assert.AreEqual(options.Region, regionParam.Value);
            var streetParam = savedRequest.Parameters.Find(x => x.Name == "Street");
            Assert.IsNotNull(streetParam);
            Assert.AreEqual(options.Street, streetParam.Value);
        }

        [Test]
        public void ShouldDeleteAddress()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new RestResponse());
            var client = mockClient.Object;

            client.DeleteAddress(ADDRESS_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull (addressSidParam);
            Assert.AreEqual (ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public void ShouldDeleteAddressAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse>>()))
                .Callback<RestRequest, Action<RestResponse>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.DeleteAddress(ADDRESS_SID, address => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull (addressSidParam);
            Assert.AreEqual (ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public void ShouldListDependentPhoneNumbers()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<DependentPhoneNumberResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new DependentPhoneNumberResult());
            var client = mockClient.Object;

            client.ListDependentPhoneNumbers(ADDRESS_SID);

            mockClient.Verify(trc => trc.Execute<DependentPhoneNumberResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}/DependentPhoneNumbers.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public void ShouldListDependentPhoneNumbersAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<DependentPhoneNumberResult>(It.IsAny<RestRequest>(), It.IsAny<Action<DependentPhoneNumberResult>>()))
                .Callback<RestRequest, Action<DependentPhoneNumberResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListDependentPhoneNumbers(ADDRESS_SID, address => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<DependentPhoneNumberResult>(It.IsAny<RestRequest>(), It.IsAny<Action<DependentPhoneNumberResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}/DependentPhoneNumbers.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
        }

    }
}