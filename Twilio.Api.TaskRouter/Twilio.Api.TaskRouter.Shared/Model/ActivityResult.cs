﻿using System;
using System.Collections.Generic;

namespace Twilio.TaskRouter
{
    /// <summary>
    /// Twilio API call result with paging information.
    /// </summary>
    public class ActivityResult : MetadataListBase
    {
        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        public List<Activity> Activities { get; set; }
    }
}

