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
        /// Retrieve the details for a workflow statistics instance. Makes a GET request to a WorkflowStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="workflowSid">The Sid of the workflow to retrieve</param>
        public virtual async Task<WorkflowStatistics> GetWorkflowStatisticsAsync(string workspaceSid, string workflowSid)
        {
            return await GetWorkflowStatisticsAsync(workspaceSid, workflowSid, new StatisticsRequest());
        }

        /// <summary>
        /// Retrieve the details for a workflow statistics instance. Makes a GET request to a WorkflowStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="workflowSid">The Sid of the workflow to retrieve</param>
        /// <param name="options">Time-interval options.</param>
        public virtual async Task<WorkflowStatistics> GetWorkflowStatisticsAsync(string workspaceSid, string workflowSid, StatisticsRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("WorkflowSid", workflowSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Workflows/{WorkflowSid}/Statistics";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("WorkflowSid", workflowSid);

            AddStatisticsDateOptions(options, request);

            return await Execute<WorkflowStatistics>(request);
        }
    }
}

