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
    public class WorkerStatisticsTests
    {
        private const string WORKER_SID = "WK123";

        private const string WORKSPACE_SID = "WS123";

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldGetWorkerStatistics()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkerStatistics>();
            tcs.SetResult(new WorkerStatistics());

            mockClient.Setup(trc => trc.Execute<WorkerStatistics>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var options = new StatisticsRequest();
            options.Minutes = 10;

            await client.GetWorkerStatisticsAsync(WORKSPACE_SID, WORKER_SID, options);

            mockClient.Verify(trc => trc.Execute<WorkerStatistics>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workers/{WorkerSid}/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workerSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerSid");
            Assert.IsNotNull(workerSidParam);
            Assert.AreEqual(WORKER_SID, workerSidParam.Value);
            var minutesParam = savedRequest.Parameters.Find(x => x.Name == "Minutes");
            Assert.IsNotNull(minutesParam);
            Assert.AreEqual(10, minutesParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkersStatistics()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkersStatistics>();
            tcs.SetResult(new WorkersStatistics());

            mockClient.Setup(trc => trc.Execute<WorkersStatistics>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListWorkersStatisticsAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<WorkersStatistics>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workers/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull (workspaceSidParam);
            Assert.AreEqual (WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkersStatisticsUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkersStatistics>();
            tcs.SetResult(new WorkersStatistics());

            mockClient.Setup(trc => trc.Execute<WorkersStatistics>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            var minutes = 10;
            var taskQueueSid = "WQ123";
            var taskQueueName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName ();
            var options = new WorkersStatisticsRequest();
            options.Minutes = minutes;
            options.FriendlyName = friendlyName;
            options.TaskQueueSid = taskQueueSid;
            options.TaskQueueName = taskQueueName;

            await client.ListWorkersStatisticsAsync(WORKSPACE_SID, options);

            mockClient.Verify(trc => trc.Execute<WorkersStatistics>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workers/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull (workspaceSidParam);
            Assert.AreEqual (WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull (friendlyNameParam);
            Assert.AreEqual (friendlyName, friendlyNameParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull (taskQueueSidParam);
            Assert.AreEqual (taskQueueSid, taskQueueSidParam.Value);
            var taskQueueNameParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueName");
            Assert.IsNotNull (taskQueueNameParam);
            Assert.AreEqual (taskQueueName, taskQueueNameParam.Value);
            var minutesParam = savedRequest.Parameters.Find(x => x.Name == "Minutes");
            Assert.IsNotNull(minutesParam);
            Assert.AreEqual(minutes, minutesParam.Value);
        }

    }
}

