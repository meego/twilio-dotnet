﻿using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Return a list of all Queue resources
        /// </summary>
        /// <returns></returns>
        public virtual async Task<QueueResult> ListQueuesAsync()
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Queues.json";

            return await Execute<QueueResult>(request);
        }

        /// <summary>
        /// Creates a new Queue resource
        /// </summary>
        /// <param name="friendlyName">The name of the Queue</param>
        /// <returns></returns>
        public virtual async Task<Queue> CreateQueueAsync(string friendlyName)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Queues.json";

            request.AddParameter("FriendlyName", friendlyName);

            return await Execute<Queue>(request);
        }

        /// <summary>
        /// Creates a new Queue resource
        /// </summary>
        /// <param name="friendlyName">The name of the Queue</param>
        /// <param name="maxSize">The maximum number of calls allowed in the queue</param>
        /// <returns></returns>
        public virtual async Task<Queue> CreateQueueAsync(string friendlyName, int maxSize)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Queues.json";

            request.AddParameter("FriendlyName", friendlyName);
            request.AddParameter("MaxSize", maxSize);

            return await Execute<Queue>(request);
        }

        /// <summary>
        /// Locates and returns a specific Queue resource
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <returns></returns>
        public virtual async Task<Queue> GetQueueAsync(string queueSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);

            return await Execute<Queue>(request);
        }

        /// <summary>
        /// Updates a specific Queue resource
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to update</param>
        /// <param name="friendlyName">The name of the Queue</param>
        /// <param name="maxSize">The maximum number of calls allowed in the queue</param>
        /// <returns></returns>
        public virtual async Task<Queue> UpdateQueueAsync(string queueSid, string friendlyName, int maxSize)
        {
            Require.Argument("QueueSid", queueSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(friendlyName)) request.AddParameter("FriendlyName", friendlyName);
            request.AddParameter("MaxSize", maxSize);

            return await Execute<Queue>(request);
        }

        /// <summary>
        /// Deletes a Queue resource
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to delete</param>
        /// <returns></returns>
        public virtual async Task<DeleteStatus> DeleteQueueAsync(string queueSid)
        {
            Require.Argument("QueueSid", queueSid);
            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Return a List of all Calls currently in the the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <returns></returns>
        public virtual async Task<QueueMemberResult> ListQueueMembersAsync(string queueSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}/Members.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);

            return await Execute<QueueMemberResult>(request);
        }

        /// <summary>
        /// Returns the Call in the first position of the wait Queue for the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <returns></returns>
        public virtual async Task<QueueMember> GetFirstQueueMemberAsync(string queueSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}/Members/Front.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);

            return await Execute<QueueMember>(request);
        }


        /// <summary>
        /// Locate and return a specific Call in the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to search</param>
        /// <param name="callSid">The Sid of the Call to locate</param>
        /// <returns></returns>
        public virtual async Task<QueueMember> GetQueueMemberAsync(string queueSid, string callSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}/Members/{CallSid}.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);
            request.AddParameter("CallSid", callSid, ParameterType.UrlSegment);

            return await Execute<QueueMember>(request);
        }

        /// <summary>
        /// Dequeues the Caller in the first wait position for the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <param name="url">A Url containing TwiML intructions to execute when the call is dequeued</param>
        /// <returns></returns>
        public virtual async Task<DequeueStatus> DequeueFirstQueueMemberAsync(string queueSid, string url)
        {
            return await DequeueFirstQueueMemberAsync(queueSid, url, string.Empty);
        }

        /// <summary>
        /// Dequeues the Caller in the first wait position for the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <param name="url">A Url containing TwiML intructions to execute when the call is dequeued</param>
        /// <param name="method">The method to use to request the Url</param>
        /// <returns></returns>
        public virtual async Task<DequeueStatus> DequeueFirstQueueMemberAsync(string queueSid, string url, string method)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}/Members/Front.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);
            request.AddParameter("Url", url);
            if (!string.IsNullOrEmpty(method)) { request.AddParameter("Method", method); }

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? DequeueStatus.Success : DequeueStatus.Failed;
        }

        /// <summary>
        /// Dequeues a specific Caller in the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <param name="callSid">The Sid of the Caller to dequeue</param>
        /// <param name="url">A Url containing TwiML intructions to execute when the call is dequeued</param>
        /// <returns></returns>
        public virtual async Task<DequeueStatus> DequeueQueueMemberAsync(string queueSid, string callSid, string url)
        {
            return await DequeueQueueMemberAsync(queueSid, callSid, url, string.Empty);
        }

        /// <summary>
        /// Dequeues a specific Caller in the specified Queue
        /// </summary>
        /// <param name="queueSid">The Sid of the Queue to locate</param>
        /// <param name="callSid">The Sid of the Caller to dequeue</param>
        /// <param name="url">A Url containing TwiML intructions to execute when the call is dequeued</param>
        /// <param name="method">The method to use to request the Url</param>
        /// <returns></returns>
        public virtual async Task<DequeueStatus> DequeueQueueMemberAsync(string queueSid, string callSid, string url, string method)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Queues/{QueueSid}/Members/{CallSid}.json";

            request.AddParameter("QueueSid", queueSid, ParameterType.UrlSegment);
            request.AddParameter("CallSid", callSid, ParameterType.UrlSegment);

            request.AddParameter("Url", url);
            if (!string.IsNullOrEmpty(method)) { request.AddParameter("Method", method); }

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? DequeueStatus.Success : DequeueStatus.Failed;
        }
    }
}
