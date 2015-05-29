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
        /// Create a worker.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="friendlyName">Friendly name.</param>
        /// <param name="activitySid">Optional activity sid.</param>
        /// <param name="attributes">Optional attributes.</param>
        public virtual async Task<Worker> AddWorkerAsync(string workspaceSid, string friendlyName, string activitySid, string attributes)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("FriendlyName", friendlyName);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/Workers";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddParameter("FriendlyName", friendlyName);
            request.AddParameter("ActivitySid", activitySid);
            request.AddParameter("Attributes", attributes);

            return await Execute<Worker>(request);
        }

        /// <summary>
        /// Delete a worker.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="workerSid">Worker sid.</param>
        public virtual async Task<DeleteStatus> DeleteWorkerAsync(string workspaceSid, string workerSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("WorkerSid", workerSid);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "Workspaces/{WorkspaceSid}/Workers/{WorkerSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("WorkerSid", workerSid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Retrieve the details for a worker instance. Makes a GET request to a Worker Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the worker belongs to</param>
        /// <param name="workerSid">The Sid of the worker to retrieve</param>
        public virtual async Task<Worker> GetWorkerAsync(string workspaceSid, string workerSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("WorkerSid", workerSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Workers/{WorkerSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("WorkerSid", workerSid);

            return await Execute<Worker>(request);
        }

        /// <summary>
        /// List workers on current workspace.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the workers belong to</param>
        public virtual async Task<WorkerResult> ListWorkersAsync(string workspaceSid)
        {
            return await ListWorkersAsync(workspaceSid, new WorkerListRequest());
        }

        /// <summary>
        /// List workers on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the workers belong to</param>
        /// <param name="options">List filter options. If an property is set the list will be filtered by that value.</param>
        public virtual async Task<WorkerResult> ListWorkersAsync(string workspaceSid, WorkerListRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Workers";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            AddWorkerListOptions(options, request);

            return await Execute<WorkerResult>(request);
        }

        /// <summary>
        /// Update a worker.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="workerSid">Worker sid.</param>
        /// <param name="activitySid">Optional activity sid.</param>
        /// <param name="attributes">Optional attributes.</param>
        /// <param name="friendlyName">Optional friendly name.</param>
        public virtual async Task<Worker> UpdateWorkerAsync(string workspaceSid, string workerSid, string activitySid, string attributes, string friendlyName)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("WorkerSid", workerSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/Workers/{WorkerSid}";
            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("WorkerSid", workerSid);

            if (activitySid.HasValue())
                request.AddParameter("ActivitySid", activitySid);
            if (attributes.HasValue())
                request.AddParameter("Attributes", attributes);
            if (friendlyName.HasValue())
                request.AddParameter("FriendlyName", friendlyName);

            return await Execute<Worker>(request);
        }

        private void AddWorkerListOptions(WorkerListRequest options, RestRequest request)
        {
            if (options.TaskQueueSid.HasValue()) request.AddParameter("TaskQueueSid", options.TaskQueueSid);
            if (options.TaskQueueName.HasValue()) request.AddParameter("TaskQueueName", options.TaskQueueName);
            if (options.ActivitySid.HasValue()) request.AddParameter("ActivitySid", options.ActivitySid);
            if (options.ActivityName.HasValue()) request.AddParameter("ActivityName", options.ActivityName);
            if (options.FriendlyName.HasValue()) request.AddParameter("FriendlyName", options.FriendlyName);
            if (options.TargetWorkersExpression.HasValue()) request.AddParameter("TargetWorkersExpression", options.TargetWorkersExpression);

            if (options.AfterSid.HasValue()) request.AddParameter("AfterSid", options.AfterSid);
            if (options.BeforeSid.HasValue()) request.AddParameter("BeforeSid", options.BeforeSid);
            if (options.Count.HasValue) request.AddParameter("PageSize", options.Count.Value);
        }
    }
}

