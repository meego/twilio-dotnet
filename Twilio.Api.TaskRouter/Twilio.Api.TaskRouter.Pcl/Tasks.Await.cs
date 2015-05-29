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
        /// Create a task.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="attributes">Attributes.</param>
        /// <param name="workflowSid">Workflow sid.</param>
        public virtual async Task<Task> AddTaskAsync(string workspaceSid, string attributes, string workflowSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("Attributes", attributes);
            Require.Argument("WorkflowSid", workflowSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddParameter("Attributes", attributes);
            request.AddParameter("WorkflowSid", workflowSid);

            return await Execute<Task>(request);
        }

        /// <summary>
        /// Delete a task.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskSid">Task sid.</param>
        public virtual async Task<DeleteStatus> DeleteTaskAsync(string workspaceSid, string taskSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskSid", taskSid);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks/{TaskSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskSid", taskSid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Retrieve the details for a task instance. Makes a GET request to a Task Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="taskSid">The Sid of the task to retrieve</param>
        public virtual async Task<Task> GetTaskAsync(string workspaceSid, string taskSid)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskSid", taskSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks/{TaskSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskSid", taskSid);

            return await Execute<Task>(request);
        }

        /// <summary>
        /// List tasks on current workspace.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the tasks belong to</param>
        public virtual async Task<TaskResult> ListTasksAsync(string workspaceSid)
        {
            return await ListTasksAsync(workspaceSid, new TaskListRequest());
        }

        /// <summary>
        /// List tasks on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the tasks belong to</param>
        /// <param name="options">List filter options. If an property is set the list will be filtered by that value.</param>
        public virtual async Task<TaskResult> ListTasksAsync(string workspaceSid, TaskListRequest options)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            AddTaskListOptions(options, request);

            return await Execute<TaskResult>(request);
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskSid">Task sid.</param>
        /// <param name="attributes">Optional attributes.</param>
        /// <param name="assignmentStatus">Optional assignment status.</param>
        /// <param name="reason">Optional reason.</param>
        public virtual async Task<Task> UpdateTaskAsync(string workspaceSid, string taskSid, string attributes, string assignmentStatus, string reason)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskSid", taskSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks/{TaskSid}";
            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskSid", taskSid);

            if (attributes.HasValue())
                request.AddParameter("Attributes", attributes);
            if (assignmentStatus.HasValue())
                request.AddParameter("AssignmentStatus", assignmentStatus);
            if (reason.HasValue())
                request.AddParameter("Reason", reason);

            return await Execute<Task>(request);
        }

        private void AddTaskListOptions(TaskListRequest options, RestRequest request)
        {
            if (options.Priority.HasValue()) request.AddParameter("Priority", options.Priority);
            if (options.AssignmentStatus.HasValue()) request.AddParameter("AssignmentStatus", options.AssignmentStatus);
            if (options.WorkflowSid.HasValue()) request.AddParameter("WorkflowSid", options.WorkflowSid);
            if (options.WorkflowName.HasValue()) request.AddParameter("WorkflowName", options.WorkflowName);
            if (options.TaskQueueSid.HasValue()) request.AddParameter("TaskQueueSid", options.TaskQueueSid);
            if (options.TaskQueueName.HasValue()) request.AddParameter("TaskQueueName", options.TaskQueueName);

            if (options.AfterSid.HasValue()) request.AddParameter("AfterSid", options.AfterSid);
            if (options.BeforeSid.HasValue()) request.AddParameter("BeforeSid", options.BeforeSid);
            if (options.Count.HasValue) request.AddParameter("PageSize", options.Count.Value);
        }
    }
}

