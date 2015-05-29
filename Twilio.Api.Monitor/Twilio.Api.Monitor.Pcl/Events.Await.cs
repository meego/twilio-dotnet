
using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Monitor
{
    public partial class MonitorClient
    {
        /// <summary>
        /// Gets an event by its unique ID.
        /// </summary>
        /// <param name="eventSid">Event sid.</param>
        public virtual async Task<Event> GetEventAsync(string eventSid)
        {
            Require.Argument("EventSid", eventSid);

            var request = new RestRequest();
            request.Resource = "Events/{EventSid}";

            request.AddUrlSegment("EventSid", eventSid);

            return await Execute<Event>(request);
        }

        /// <summary>
        /// Lists the events.
        /// </summary>
        public virtual async Task<EventResult> ListEventsAsync()
        {
            return await ListEventsAsync(new EventListRequest());
        }

        /// <summary>
        /// Lists the events.
        /// </summary>
        /// <param name="options">Options.</param>
        public virtual async Task<EventResult> ListEventsAsync(EventListRequest options)
        {
            var request = new RestRequest();
            request.Resource = "Events";

            AddEventListOptions(options, request);

            return await Execute<EventResult>(request);
        }

        private void AddEventListOptions(EventListRequest options, RestRequest request)
        {
            if (options.StartDate.HasValue) {
                request.AddParameter("StartDate", options.StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }
            if (options.EndDate.HasValue) {
                request.AddParameter("EndDate", options.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }
            if (options.ActorSid.HasValue()) {
                request.AddParameter("ActorSid", options.ActorSid);
            }
            if (options.SourceIpAddress.HasValue()) {
                request.AddParameter("SourceIpAddress", options.SourceIpAddress);
            }
            if (options.ResourceSid.HasValue()) {
                request.AddParameter("ResourceSid", options.ResourceSid);
            }
            if (options.EventType.HasValue()) {
                request.AddParameter("EventType", options.EventType);
            }
            if (options.PageToken.HasValue()) {
                request.AddParameter("PageToken", options.PageToken);
            }
            if (options.Count.HasValue) {
                request.AddParameter("PageSize", options.Count.Value);
            }
        }
    }
}

