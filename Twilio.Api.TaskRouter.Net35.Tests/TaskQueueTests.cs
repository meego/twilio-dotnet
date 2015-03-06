using System;
using System.Threading;
using Moq;
using NUnit.Framework;
//using RestSharp;
//using RestSharp.Deserializers;
//using Simple;

using Twilio.TaskRouter;
using Twilio.Api.Tests.Integration;
using Twilio.Api.Tests;
using Simple;

namespace Twilio.TaskRouter.Tests
{
    [TestFixture]
    public class TaskQueueTests
    {
        private const string TASK_QUEUE_SID = "WQ123";

        private const string WORKSPACE_SID = "WS123";

        ManualResetEvent manualResetEvent = null;

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldAddNewTaskQueue()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new TaskQueue());
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.AddTaskQueue(WORKSPACE_SID, friendlyName, "WA123", "WA234");

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentActivitySid");
            Assert.IsNotNull(assignmentActivitySidParam);
            Assert.AreEqual("WA123", assignmentActivitySidParam.Value);
            var reservationActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationActivitySid");
            Assert.IsNotNull(reservationActivitySidParam);
            Assert.AreEqual("WA234", reservationActivitySidParam.Value);
        }

        [Test]
        public void ShouldAddNewTaskQueueAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()))
                .Callback<RestRequest, Action<TaskQueue>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.AddTaskQueue(WORKSPACE_SID, friendlyName, "WA123", "WA234", taskQueue =>
                {
                    manualResetEvent.Set();
                });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentActivitySid");
            Assert.IsNotNull(assignmentActivitySidParam);
            Assert.AreEqual("WA123", assignmentActivitySidParam.Value);
            var reservationActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationActivitySid");
            Assert.IsNotNull(reservationActivitySidParam);
            Assert.AreEqual("WA234", reservationActivitySidParam.Value);
        }

        [Test]
        public void ShouldDeleteTaskQueue()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new RestResponse());
            var client = mockClient.Object;

            client.DeleteTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
        }

        [Test]
        public void ShouldDeleteTaskQueueAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse>>()))
                .Callback<RestRequest, Action<RestResponse>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.DeleteTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID, status => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
        }

        [Test]
        public void ShouldGetTaskQueue()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new TaskQueue());
            var client = mockClient.Object;

            client.GetTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID);

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
        }

        [Test]
        public void ShouldGetTaskQueueAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()))
                .Callback<RestRequest, Action<TaskQueue>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID, taskQueue => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
        }

        [Test]
        public void ShouldListTaskQueues()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new TaskQueueResult());
            var client = mockClient.Object;

            client.ListTaskQueues(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public void ShouldListTaskQueuesAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<TaskQueueResult>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueueResult>>()))
                .Callback<RestRequest, Action<TaskQueueResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListTaskQueues(WORKSPACE_SID, taskQueues => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<TaskQueueResult>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueueResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public void ShouldListTaskQueuesUsingFilters()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new TaskQueueResult());
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.ListTaskQueues(WORKSPACE_SID, friendlyName, "evaluateWorkerAttributes", "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var evaluateWorkerAttributesParam = savedRequest.Parameters.Find(x => x.Name == "EvaluateWorkerAttributes");
            Assert.IsNotNull(evaluateWorkerAttributesParam);
            Assert.AreEqual("evaluateWorkerAttributes", evaluateWorkerAttributesParam.Value);
            var afterSidParam = savedRequest.Parameters.Find(x => x.Name == "AfterSid");
            Assert.IsNotNull(afterSidParam);
            Assert.AreEqual("afterSid", afterSidParam.Value);
            var beforeSidParam = savedRequest.Parameters.Find(x => x.Name == "BeforeSid");
            Assert.IsNotNull(beforeSidParam);
            Assert.AreEqual("beforeSid", beforeSidParam.Value);
            var countSidParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countSidParam);
            Assert.AreEqual(10, countSidParam.Value);
        }

        [Test]
        public void ShouldListTaskQueuesUsingFiltersAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<TaskQueueResult>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueueResult>>()))
                .Callback<RestRequest, Action<TaskQueueResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.ListTaskQueues(WORKSPACE_SID, friendlyName, "evaluateWorkerAttributes", "afterSid", "beforeSid", 10, taskQueues => {
                manualResetEvent.Set();
            });

            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<TaskQueueResult>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueueResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var evaluateWorkerAttributesParam = savedRequest.Parameters.Find(x => x.Name == "EvaluateWorkerAttributes");
            Assert.IsNotNull(evaluateWorkerAttributesParam);
            Assert.AreEqual("evaluateWorkerAttributes", evaluateWorkerAttributesParam.Value);
            var afterSidParam = savedRequest.Parameters.Find(x => x.Name == "AfterSid");
            Assert.IsNotNull(afterSidParam);
            Assert.AreEqual("afterSid", afterSidParam.Value);
            var beforeSidParam = savedRequest.Parameters.Find(x => x.Name == "BeforeSid");
            Assert.IsNotNull(beforeSidParam);
            Assert.AreEqual("beforeSid", beforeSidParam.Value);
            var countSidParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countSidParam);
            Assert.AreEqual(10, countSidParam.Value);
        }

        [Test]
        public void ShouldUpdateTaskQueue()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new TaskQueue());
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.UpdateTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID, friendlyName, "WA123", "WA234", "targetWorkers");

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentActivitySid");
            Assert.IsNotNull(assignmentActivitySidParam);
            Assert.AreEqual("WA123", assignmentActivitySidParam.Value);
            var reservationActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationActivitySid");
            Assert.IsNotNull(reservationActivitySidParam);
            Assert.AreEqual("WA234", reservationActivitySidParam.Value);
            var targetWorkersParam = savedRequest.Parameters.Find(x => x.Name == "TargetWorkers");
            Assert.IsNotNull(targetWorkersParam);
            Assert.AreEqual("targetWorkers", targetWorkersParam.Value);
        }

        [Test]
        public void ShouldUpdateTaskQueueAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()))
                .Callback<RestRequest, Action<TaskQueue>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            client.UpdateTaskQueue(WORKSPACE_SID, TASK_QUEUE_SID, friendlyName, "WA123", "WA234", "targetWorkers", taskQueue => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<TaskQueue>(It.IsAny<RestRequest>(), It.IsAny<Action<TaskQueue>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentActivitySid");
            Assert.IsNotNull(assignmentActivitySidParam);
            Assert.AreEqual("WA123", assignmentActivitySidParam.Value);
            var reservationActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationActivitySid");
            Assert.IsNotNull(reservationActivitySidParam);
            Assert.AreEqual("WA234", reservationActivitySidParam.Value);
            var targetWorkersParam = savedRequest.Parameters.Find(x => x.Name == "TargetWorkers");
            Assert.IsNotNull(targetWorkersParam);
            Assert.AreEqual("targetWorkers", targetWorkersParam.Value);
        }
    }
}

