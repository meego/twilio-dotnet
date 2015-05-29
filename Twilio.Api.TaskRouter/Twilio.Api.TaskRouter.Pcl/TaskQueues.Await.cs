﻿using System;
using Simple;
using System.Threading.Tasks;
//using Simple.Extensions;
//using Simple.Validation;

namespace Twilio.TaskRouter
{
    public partial class TaskRouterClient
    {
        /// <summary>
        /// Create a task queue.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="friendlyName">Friendly name.</param>
        /// <param name="assignmentActivitySid">Assignment activity sid.</param>
        /// <param name="reservationActivitySid">Reservation activity sid.</param>
        public virtual async Task<TaskQueue> AddTaskQueueAsync(string workspaceSid, string friendlyName, string assignmentActivitySid, string reservationActivitySid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("FriendlyName", friendlyName);
            Require.Argument("AssignmentActivitySid", assignmentActivitySid);
            Require.Argument("ReservationActivitySid", reservationActivitySid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/TaskQueues";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddParameter("FriendlyName", friendlyName);
            request.AddParameter("AssignmentActivitySid", assignmentActivitySid);
            request.AddParameter("ReservationActivitySid", reservationActivitySid);

            return await Execute<TaskQueue>(request);
        }

        /// <summary>
        /// Delete a task queue.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskQueueSid">Task queue sid.</param>
        public virtual async Task<DeleteStatus> DeleteTaskQueueAsync(string workspaceSid, string taskQueueSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskQueueSid", taskQueueSid);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskQueueSid", taskQueueSid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Retrieve the details for a task queue instance. Makes a GET request to a TaskQueue Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="taskQueueSid">The Sid of the task queue to retrieve</param>
        public virtual async Task<TaskQueue> GetTaskQueueAsync(string workspaceSid, string taskQueueSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskQueueSid", taskQueueSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskQueueSid", taskQueueSid);

            return await Execute<TaskQueue>(request);
        }

        /// <summary>
        /// List task queues on current workspace.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the task queues belong to</param>
        public virtual async Task<TaskQueueResult> ListTaskQueuesAsync(string workspaceSid)
        {
            return await ListTaskQueuesAsync(workspaceSid, null, null, null, null, null);
        }

        /// <summary>
        /// List task queues on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the task queues belong to</param>
        /// <param name="friendlyName">Optional friendly name to match.</param>
        /// <param name="evaluateWorkerAttributes">Optional evaluate worker attributes to match.</param>
        /// <param name="afterSid">Activity Sid to start retrieving results from</param>
        /// <param name="beforeSid">Activity Sid to stop retrieving results from</param>
        /// <param name="count">How many results to return</param>
        public virtual async Task<TaskQueueResult> ListTaskQueuesAsync(string workspaceSid, string friendlyName, string evaluateWorkerAttributes, string afterSid, string beforeSid, int? count)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/TaskQueues";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            if (friendlyName.HasValue())
                request.AddParameter("FriendlyName", friendlyName);
            if (evaluateWorkerAttributes.HasValue())
                request.AddParameter("EvaluateWorkerAttributes", evaluateWorkerAttributes);
            if (afterSid.HasValue())
                request.AddParameter("AfterSid", afterSid);
            if (beforeSid.HasValue())
                request.AddParameter("BeforeSid", beforeSid);
            if (count.HasValue)
                request.AddParameter("PageSize", count.Value);

            return await Execute<TaskQueueResult>(request);
        }

        /// <summary>
        /// Update a task queue.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskQueueSid">Task queue sid.</param>
        /// <param name="friendlyName">Optional friendly name to match.</param>
        /// <param name="assignmentActivitySid">Optional assignment activity sid.</param>
        /// <param name="reservationActivitySid">Optional reservation activity sid.</param>
        /// <param name="targetWorkers">Optional target workers.</param>
        public virtual async Task<TaskQueue> UpdateTaskQueueAsync(string workspaceSid, string taskQueueSid, string friendlyName, string assignmentActivitySid, string reservationActivitySid, string targetWorkers)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskQueueSid", taskQueueSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}";
            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskQueueSid", taskQueueSid);

            if (friendlyName.HasValue())
                request.AddParameter("FriendlyName", friendlyName);
            if (assignmentActivitySid.HasValue())
                request.AddParameter("AssignmentActivitySid", assignmentActivitySid);
            if (reservationActivitySid.HasValue())
                request.AddParameter("ReservationActivitySid", reservationActivitySid);
            if (targetWorkers.HasValue())
                request.AddParameter("TargetWorkers", targetWorkers);

            return await Execute<TaskQueue>(request);
        }
    }
}

