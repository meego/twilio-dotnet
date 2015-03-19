using System;
using System.Threading;
using Moq;
using NUnit.Framework;
//using RestSharp;
//using RestSharp.Deserializers;
//using Simple;

using Twilio.TaskRouter;
using Twilio.Api.Tests;
using Simple;
using System.Threading.Tasks;

namespace Twilio.TaskRouter.Tests
{
    [TestFixture]
    public class TaskQueueTests
    {
        private const string TASK_QUEUE_SID = "WQ123";

        private const string WORKSPACE_SID = "WS123";

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldAddNewTaskQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueue>();
            tcs.SetResult(new TaskQueue());

            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddTaskQueueAsync(WORKSPACE_SID, friendlyName, "WA123", "WA234");

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
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
        public async System.Threading.Tasks.Task ShouldDeleteTaskQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteTaskQueueAsync(WORKSPACE_SID, TASK_QUEUE_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
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
        public async System.Threading.Tasks.Task ShouldGetTaskQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueue>();
            tcs.SetResult(new TaskQueue());

            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetTaskQueueAsync(WORKSPACE_SID, TASK_QUEUE_SID);

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
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
        public async System.Threading.Tasks.Task ShouldListTaskQueues()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueueResult>();
            tcs.SetResult(new TaskQueueResult ());

            mockClient.Setup(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListTaskQueuesAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListTaskQueuesUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueueResult>();
            tcs.SetResult(new TaskQueueResult());

            mockClient.Setup(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.ListTaskQueuesAsync(WORKSPACE_SID, friendlyName, "evaluateWorkerAttributes", "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<TaskQueueResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues", savedRequest.Resource);
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
        public async System.Threading.Tasks.Task ShouldUpdateTaskQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueue>();
            tcs.SetResult(new TaskQueue());

            mockClient.Setup(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.UpdateTaskQueueAsync(WORKSPACE_SID, TASK_QUEUE_SID, friendlyName, "WA123", "WA234", "targetWorkers");

            mockClient.Verify(trc => trc.Execute<TaskQueue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}", savedRequest.Resource);
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

