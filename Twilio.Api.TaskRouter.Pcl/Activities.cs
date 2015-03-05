using System;
using Simple;
using Twilio;
using System.Threading.Tasks;
//using Simple.Extensions;
//using Simple.Validation;

namespace Twilio.TaskRouter
{
    public partial class TaskRouterClient : TwilioRestClient
    {
        /// <summary>
        /// Create an activity.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="friendlyName">Friendly name.</param>
        /// <param name="available">Optional available.</param>
        public virtual async Task<Activity> AddActivityAsync(string workspaceSid, string friendlyName, bool available)
        {
            //Require.Argument("WorkspaceSid", workspaceSid);
            //Require.Argument("FriendlyName", friendlyName);

            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddParameter("FriendlyName", friendlyName);
            request.AddParameter("Available", available);

            return await Execute<Activity>(request);
        }

        /// <summary>
        /// Delete an activity.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="activitySid">Activity sid.</param>
        public virtual async Task<DeleteStatus> DeleteActivityAsync(string workspaceSid, string activitySid)
        {
            //Require.Argument("WorkspaceSid", workspaceSid);
            //Require.Argument("ActivitySid", activitySid);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("ActivitySid", activitySid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Retrieve the details for an activity instance. Makes a GET request to an Activity Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="activitySid">The Sid of the activity to retrieve</param>
        public virtual async Task<Activity> GetActivityAsync(string workspaceSid, string activitySid)
        {
            //Require.Argument("WorkspaceSid", workspaceSid);
            //Require.Argument("ActivitySid", activitySid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("ActivitySid", activitySid);

            return await Execute<Activity>(request);
        }

        /// <summary>
        /// List activities on current workspace.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activities belong to</param>
        public virtual async Task<ActivityResult> ListActivitiesAsync(string workspaceSid)
        {
            return await ListActivitiesAsync(workspaceSid, null, null, null, null, null);
        }

        /// <summary>
        /// List activities on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activities belong to</param>
        /// <param name="available">Optional available to match</param>
        /// <param name="friendlyName">Optional friendly name to match</param>
        /// <param name="afterSid">Activity Sid to start retrieving results from</param>
        /// <param name="beforeSid">Activity Sid to stop retrieving results from</param>
        /// <param name="count">How many results to return</param>
        public virtual async Task<ActivityResult> ListActivitiesAsync(string workspaceSid, bool? available, string friendlyName, string afterSid, string beforeSid, int? count)
        {
            //Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities.json";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            if (available.HasValue)
                request.AddParameter("Available", available.Value);
            if (friendlyName.HasValue())
                request.AddParameter("FriendlyName", friendlyName);
            if (afterSid.HasValue())
                request.AddParameter("AfterSid", afterSid);
            if (beforeSid.HasValue())
                request.AddParameter("BeforeSid", beforeSid);
            if (count.HasValue)
                request.AddParameter("PageSize", count.Value);

            return await Execute<ActivityResult>(request);
        }

        /// <summary>
        /// Update an activity.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="activitySid">Activity sid.</param>
        /// <param name="friendlyName">Optional friendly name.</param>
        /// <param name="available">Optional available.</param>
        public virtual async Task<Activity> UpdateActivityAsync(string workspaceSid, string activitySid, string friendlyName, bool? available)
        {
            //Require.Argument("WorkspaceSid", workspaceSid);
            //Require.Argument("ActivitySid", activitySid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json";
            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("ActivitySid", activitySid);

            if (friendlyName.HasValue())
                request.AddParameter("FriendlyName", friendlyName);
            if (available.HasValue)
                request.AddParameter("Available", available.Value);

            return await Execute<Activity>(request);
        }
    }
}
