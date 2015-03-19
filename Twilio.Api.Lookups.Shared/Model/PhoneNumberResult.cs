using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio.Lookups
{
    public class PhoneNumberResult : MetadataListBase
    {
        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        public List<PhoneNumber> PhoneNumbers { get; set; }

    }
}
