using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twilio
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataListBase : TwilioBase
    {
        /// <summary>
        /// Metadata about this list resource representation.
        /// Includes resource name and paging information.
        /// </summary>
        public ListMetadata Meta { get; set; }

    }
}