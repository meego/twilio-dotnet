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
    public class TaskQueueStatisticsTests
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
        public async System.Threading.Tasks.Task ShouldGetTaskQueueStatistics()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueueStatistics>();
            tcs.SetResult(new TaskQueueStatistics());

            mockClient.Setup(trc => trc.Execute<TaskQueueStatistics>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            var options = new StatisticsRequest();
            options.Minutes = 10;

            await client.GetTaskQueueStatisticsAsync(WORKSPACE_SID, TASK_QUEUE_SID, options);

            mockClient.Verify(trc => trc.Execute<TaskQueueStatistics>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/{TaskQueueSid}/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(TASK_QUEUE_SID, taskQueueSidParam.Value);
            var minutesParam = savedRequest.Parameters.Find(x => x.Name == "Minutes");
            Assert.IsNotNull(minutesParam);
            Assert.AreEqual(10, minutesParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListTaskQueueStatistics()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueueStatisticsResult>();
            tcs.SetResult(new TaskQueueStatisticsResult());

            mockClient.Setup(trc => trc.Execute<TaskQueueStatisticsResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListTaskQueuesStatisticsAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<TaskQueueStatisticsResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull (workspaceSidParam);
            Assert.AreEqual (WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListTaskQueueStatisticsUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TaskQueueStatisticsResult>();
            tcs.SetResult(new TaskQueueStatisticsResult());

            mockClient.Setup(trc => trc.Execute<TaskQueueStatisticsResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            var minutes = 10;

            var options = new TaskQueuesStatisticsRequest();
            options.FriendlyName = friendlyName;
            options.Minutes = minutes;
            await client.ListTaskQueuesStatisticsAsync(WORKSPACE_SID, options);

            mockClient.Verify(trc => trc.Execute<TaskQueueStatisticsResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/TaskQueues/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull (workspaceSidParam);
            Assert.AreEqual (WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull (friendlyNameParam);
            Assert.AreEqual (friendlyName, friendlyNameParam.Value);
            var minutesParam = savedRequest.Parameters.Find(x => x.Name == "Minutes");
            Assert.IsNotNull(minutesParam);
            Assert.AreEqual(minutes, minutesParam.Value);
        }

    }
}

