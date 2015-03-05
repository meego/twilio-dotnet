using System;
using System.Collections.Generic;

namespace Twilio.TaskRouter
{
    /// <summary>
    /// Twilio API call result with paging information.
    /// </summary>
    public class ReservationResult : MetadataListBase
    {
        /// <summary>
        /// Gets or sets the reservations.
        /// </summary>
        public List<Reservation> Reservations { get; set; }
    }
}

