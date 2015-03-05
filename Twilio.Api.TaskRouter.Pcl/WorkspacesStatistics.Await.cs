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
        /// Retrieve the details for a workspace statistics instance. Makes a GET request to a WorkspaceStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        public virtual async Task<WorkspaceStatistics> GetWorkspaceStatisticsAsync(string workspaceSid)
        {
            return await GetWorkspaceStatisticsAsync(workspaceSid, new StatisticsRequest());
        }

        /// <summary>
        /// Retrieve the details for a workspace statistics instance. Makes a GET request to a WorkspaceStatistics Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="options">Time-interval options.</param>
        public virtual async Task<WorkspaceStatistics> GetWorkspaceStatisticsAsync(string workspaceSid, StatisticsRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Statistics.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            AddStatisticsDateOptions(options, request);

            return await Execute<WorkspaceStatistics>(request);
        }
    }
}

