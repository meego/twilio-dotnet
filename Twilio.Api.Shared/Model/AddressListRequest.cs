using System;

namespace Twilio
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressListRequest
    {
        /// <summary>
        /// Only show Addresses with this friendly name.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Only show Addresses with this customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Only show Addresses located in this country.
        /// </summary>
        public string IsoCountry { get; set; }

        /// <summary>
        /// The page to reterive
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// The number of records per page
        /// </summary>
        public int? PageSize { get; set; }
    }
}
