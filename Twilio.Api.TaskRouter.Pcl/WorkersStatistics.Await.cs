using System;
using Simple;
using System.Threading.Tasks;
//using Simple.Extensions;
//using Simple.Validation;

namespace Twilio.TaskRouter
{
    public partial class TaskRouterClient
    {
        /// <summary>
        /// Retrieve the details for a worker statistics instance. Makes a GET request to a WorkerStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="workerSid">The Sid of the worker to retrieve</param>
        public virtual async Task<WorkerStatistics> GetWorkerStatisticsAsync(string workspaceSid, string workerSid)
        {
            return await GetWorkerStatisticsAsync(workspaceSid, workerSid, new StatisticsRequest());
        }

        /// <summary>
        /// Retrieve the details for a worker statistics instance. Makes a GET request to a WorkerStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="workerSid">The Sid of the worker to retrieve</param>
        /// <param name="options">Filtering options for statistics</param>
        public virtual async Task<WorkerStatistics> GetWorkerStatisticsAsync(string workspaceSid, string workerSid, StatisticsRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("WorkerSid", workerSid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers/{WorkerSid}/Statistics.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("WorkerSid", workerSid);

            AddStatisticsDateOptions(options, request);

            return await Execute<WorkerStatistics>(request);
        }

        /// <summary>
        /// List workers statistics on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the task queues belong to</param>
        public virtual async Task<WorkersStatistics> ListWorkersStatisticsAsync(string workspaceSid)
        {
            return await ListWorkersStatisticsAsync(workspaceSid, new WorkersStatisticsRequest());
        }

        /// <summary>
        /// List workers statistics on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the task queues belong to</param>
        /// <param name="options">Filtering options for the statistics request</param>> 
        public virtual async Task<WorkersStatistics> ListWorkersStatisticsAsync(string workspaceSid, WorkersStatisticsRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers/Statistics.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            AddWorkersStatisticsOptions(options, request);

            return await Execute<WorkersStatistics>(request);
        }

        private void AddWorkersStatisticsOptions(WorkersStatisticsRequest options, RestRequest request)
        {
            AddStatisticsDateOptions(options, request);
            if (options.FriendlyName.HasValue()) {
                request.AddParameter("FriendlyName", options.FriendlyName);
            }
            if (options.TaskQueueSid.HasValue()) {
                request.AddParameter("TaskQueueSid", options.TaskQueueSid);
            }
            if (options.TaskQueueName.HasValue()) {
                request.AddParameter("TaskQueueName", options.TaskQueueName);
            }
        }
    }
}

