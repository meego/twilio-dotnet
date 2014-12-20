using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using System.Linq;
using Twilio.Api.Tests;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class AddressTests
    {
        private const string ADDRESS_SID = "AD123";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldAddNewAddress()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Address>();
            tcs.SetResult(new Address());

            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddAddressAsync(friendlyName, "Homer Simpson", "742 Evergreen Terrace", "Springfield", "MO", "65801", "US");

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
        public async Task ShouldGetAddress()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Address>();
            tcs.SetResult(new Address());

            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetAddressAsync(ADDRESS_SID);

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
        public async Task ShouldListAddresses()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<AddressResult>();
            tcs.SetResult(new AddressResult());

            mockClient.Setup(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListAddressesAsync();

            mockClient.Verify(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListAddressesUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<AddressResult>();
            tcs.SetResult(new AddressResult());

            mockClient.Setup(trc => trc.Execute<AddressResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var options = new AddressListRequest();
            options.CustomerName = "Homer Simpson";
            options.FriendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            options.IsoCountry = "US";
            options.Page = 1;
            options.PageSize = 10;

            await client.ListAddressesAsync(options);

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
        public async Task ShouldUpdateAddress()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Address>();
            tcs.SetResult(new Address());

            mockClient.Setup(trc => trc.Execute<Address>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var options = new AddressOptions();
            options.City = "Springfield";
            options.CustomerName = "Homer Simpson";
            options.FriendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            options.PostalCode = "65801";
            options.Region = "MO";
            options.Street = "742 Evergreen Terrace";

            await client.UpdateAddressAsync(ADDRESS_SID, options);

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
        public async Task ShouldDeleteAddress()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteAddressAsync(ADDRESS_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Addresses/{AddressSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var addressSidParam = savedRequest.Parameters.Find(x => x.Name == "AddressSid");
            Assert.IsNotNull(addressSidParam);
            Assert.AreEqual(ADDRESS_SID, addressSidParam.Value);
        }

        [Test]
        public async Task ShouldListDependentPhoneNumbers()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<DependentPhoneNumberResult>();
            tcs.SetResult(new DependentPhoneNumberResult());

            mockClient.Setup(trc => trc.Execute<DependentPhoneNumberResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;

            await client.ListDependentPhoneNumbersAsync(ADDRESS_SID);

            mockClient.Verify(trc => trc.Execute<DependentPhoneNumberResult>(It.IsAny<RestRequest>()), Times.Once);
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