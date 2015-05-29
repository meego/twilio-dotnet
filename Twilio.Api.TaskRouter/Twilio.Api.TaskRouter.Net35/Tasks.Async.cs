﻿using System;
using Simple;
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
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void AddTask(string workspaceSid, string attributes, string workflowSid, Action<Task> callback)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("Attributes", attributes);
            Require.Argument("WorkflowSid", workflowSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddParameter("Attributes", attributes);
            request.AddParameter("WorkflowSid", workflowSid);

            ExecuteAsync<Task>(request, (response) => { callback(response); });
        }

        /// <summary>
        /// Delete a task.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskSid">Task sid.</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void DeleteTask(string workspaceSid, string taskSid, Action<DeleteStatus> callback)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskSid", taskSid);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks/{TaskSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskSid", taskSid);

            ExecuteAsync(request, (response) => { callback(response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed); });
        }

        /// <summary>
        /// Retrieve the details for a task instance. Makes a GET request to a Task Instance resource.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the activity belongs to</param>
        /// <param name="taskSid">The Sid of the task to retrieve</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void GetTask(string workspaceSid, string taskSid, Action<Task> callback)
        {
            Require.Argument("WorkspaceSid", workspaceSid);
            Require.Argument("TaskSid", taskSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks/{TaskSid}";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);
            request.AddUrlSegment("TaskSid", taskSid);

            ExecuteAsync<Task>(request, (response) => { callback(response); });
        }

        /// <summary>
        /// List tasks on current workspace.
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the tasks belong to</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListTasks(string workspaceSid, Action<TaskResult> callback)
        {
            ListTasks(workspaceSid, new TaskListRequest(), callback);
        }

        /// <summary>
        /// List tasks on current workspace with filters
        /// </summary>
        /// <param name="workspaceSid">The Sid of the workspace the tasks belong to</param>
        /// <param name="options">List filter options. If an property is set the list will be filtered by that value.</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListTasks(string workspaceSid, TaskListRequest options, Action<TaskResult> callback)
        {
            Require.Argument("WorkspaceSid", workspaceSid);

            var request = new RestRequest();
            request.Resource = "Workspaces/{WorkspaceSid}/Tasks";

            request.AddUrlSegment("WorkspaceSid", workspaceSid);

            AddTaskListOptions(options, request);

            ExecuteAsync<TaskResult>(request, callback);
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        /// <param name="workspaceSid">Workspace sid.</param>
        /// <param name="taskSid">Task sid.</param>
        /// <param name="attributes">Optional attributes.</param>
        /// <param name="assignmentStatus">Optional assignment status.</param>
        /// <param name="reason">Optional reason.</param>
        /// <param name="callback">Method to call upon successful completion</param>
        public virtual void UpdateTask(string workspaceSid, string taskSid, string attributes, string assignmentStatus, string reason, Action<Task> callback)
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

            ExecuteAsync<Task>(request, (response) => { callback(response); });
        }
    }
}

