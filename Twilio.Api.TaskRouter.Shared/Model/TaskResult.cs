using System;
using System.Collections.Generic;

namespace Twilio.TaskRouter
{
    /// <summary>
    /// Twilio API call result with paging information.
    /// </summary>
    public class TaskResult : MetadataListBase
    {
        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        public List<Task> Tasks { get; set; }
    }
}

