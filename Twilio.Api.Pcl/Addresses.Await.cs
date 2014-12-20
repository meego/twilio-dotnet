using System;
using Simple;
using System.Threading.Tasks;
//using Simple.Extensions;
//using Simple.Validation;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Retrieve the details for an address instance. Makes a GET request to an Address Instance resource.
        /// </summary>
        /// <param name="addressSid">The Sid of the address to retrieve</param>
        public virtual async Task<Address> GetAddressAsync(string addressSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Addresses/{AddressSid}.json";

            request.AddUrlSegment("AddressSid", addressSid);

            return await Execute<Address>(request);
        }

        /// <summary>
        /// List Addresses on the current account.
        /// </summary>
        public virtual async Task<AddressResult> ListAddressesAsync()
        {
            return await ListAddressesAsync(new AddressListRequest());
        }

        /// <summary>
        /// List Addresses on the current account, with filters.
        /// </summary>
        /// <param name="options">Filters to be applied to the request.</param>
        public virtual async Task<AddressResult> ListAddressesAsync(AddressListRequest options)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Addresses.json";

            AddAddressListOptions(options, request);

            return await Execute<AddressResult>(request);
        }

        /// <summary>
        /// List the current account's incoming phone numbers that depend on a specific address.
        /// </summary>
        /// <param name="addressSid">Sid of the address to retrieve dependent phone numbers for.</param>
        public virtual async Task<DependentPhoneNumberResult> ListDependentPhoneNumbersAsync(string addressSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Addresses/{AddressSid}/DependentPhoneNumbers.json";

            request.AddUrlSegment("AddressSid", addressSid);

            return await Execute<DependentPhoneNumberResult>(request);
        }

        /// <summary>
        /// Create a new Address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="friendlyName">Friendly name (optional)</param>
        /// <param name="customerName">Customer name</param>
        /// <param name="street">Number and street of the address</param>
        /// <param name="city">City name for the address</param>
        /// <param name="region">The state or region</param>
        /// <param name="postalCode">Postal code</param>
        /// <param name="isoCountry">ISO 3166-1 2-letter country code</param>
        public virtual async Task<Address> AddAddressAsync(string friendlyName, string customerName, string street, string city, string region, string postalCode, string isoCountry)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Addresses.json";

            Require.Argument("CustomerName", customerName);
            Require.Argument("Street", street);
            Require.Argument("City", city);
            Require.Argument("Region", region);
            Require.Argument("PostalCode", postalCode);
            Require.Argument("IsoCountry", isoCountry);

            if (friendlyName.HasValue())
            {
                Validate.IsValidLength(friendlyName, 64);
                request.AddParameter("FriendlyName", friendlyName);
            }

            request.AddParameter("CustomerName", customerName);
            request.AddParameter("Street", street);
            request.AddParameter("City", city);
            request.AddParameter("Region", region);
            request.AddParameter("PostalCode", postalCode);
            request.AddParameter("IsoCountry", isoCountry);

            return await Execute<Address>(request);
        }

        /// <summary>
        /// Update an Address. All attributes of the address except for the IsoCountry can be updated.
        /// </summary>
        /// <returns>Updated Address object.</returns>
        /// <param name="addressSid">The sid of the Address to update.</param>
        /// <param name="options">Which properties to update. Only properties with values set will be updated.</param>
        public virtual async Task<Address> UpdateAddressAsync(string addressSid, AddressOptions options)
        {
            Require.Argument("AddressSid", addressSid);
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Addresses/{AddressSid}.json";
            request.AddUrlSegment("AddressSid", addressSid);

            if (options.FriendlyName.HasValue())
                request.AddParameter("FriendlyName", options.FriendlyName);
            if (options.CustomerName.HasValue())
                request.AddParameter("CustomerName", options.CustomerName);
            if (options.Street.HasValue())
                request.AddParameter("Street", options.Street);
            if (options.City.HasValue())
                request.AddParameter("City", options.City);
            if (options.Region.HasValue())
                request.AddParameter("Region", options.Region);
            if (options.PostalCode.HasValue())
                request.AddParameter("PostalCode", options.PostalCode);

            return await Execute<Address>(request);
        }

        /// <summary>
        /// Delete an Address.
        /// </summary>
        /// <returns>A DeleteStatus object indicating whether the request succeeded.</returns>
        /// <param name="addressSid">The sid of the Address to be deleted.</param>
        public virtual async Task<DeleteStatus> DeleteAddressAsync(string addressSid)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Addresses/{AddressSid}.json";
            request.AddUrlSegment("AddressSid", addressSid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        private void AddAddressListOptions(AddressListRequest options, RestRequest request)
        {
            if (options.CustomerName.HasValue()) request.AddParameter("CustomerName", options.CustomerName);
            if (options.FriendlyName.HasValue()) request.AddParameter("FriendlyName", options.FriendlyName);
            if (options.IsoCountry.HasValue()) request.AddParameter("IsoCountry", options.IsoCountry);

            if (options.Page.HasValue) request.AddParameter("Page", options.Page);
            if (options.PageSize.HasValue) request.AddParameter("PageSize", options.PageSize);
        }
    }
}
