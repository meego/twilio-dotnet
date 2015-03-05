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
    public class WorkerTests
    {
        private const string WORKER_SID = "WK123";

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
        public async System.Threading.Tasks.Task ShouldAddNewWorker()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Worker>();
            tcs.SetResult(new Worker());

            mockClient.Setup(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddWorkerAsync(WORKSPACE_SID, friendlyName, "WA123", "attributes");

            mockClient.Verify(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual("WA123", activitySidParam.Value);
            var attributesParam = savedRequest.Parameters.Find(x => x.Name == "Attributes");
            Assert.IsNotNull(attributesParam);
            Assert.AreEqual("attributes", attributesParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteWorker()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteWorkerAsync(WORKSPACE_SID, WORKER_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers/{WorkerSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
            var workerSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerSid");
            Assert.IsNotNull(workerSidParam);
            Assert.AreEqual(WORKER_SID, workerSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldGetWorker()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Worker>();
            tcs.SetResult(new Worker());

            mockClient.Setup(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetWorkerAsync(WORKSPACE_SID, WORKER_SID);

            mockClient.Verify(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers/{WorkerSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workerSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerSid");
            Assert.IsNotNull(workerSidParam);
            Assert.AreEqual(WORKER_SID, workerSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkers()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkerResult>();
            tcs.SetResult(new WorkerResult());

            mockClient.Setup(trc => trc.Execute<WorkerResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListWorkersAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<WorkerResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkersUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkerResult>();
            tcs.SetResult(new WorkerResult());

            mockClient.Setup(trc => trc.Execute<WorkerResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var options = new WorkerListRequest();
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();
            options.FriendlyName = friendlyName;
            options.ActivityName = "activityName";
            options.ActivitySid = "WA123";
            options.AfterSid = "afterSid";
            options.BeforeSid = "beforeSid";
            options.Count = 10;
            options.TargetWorkersExpression = "expression";
            options.TaskQueueName = "taskQueueName";
            options.TaskQueueSid = "WQ123";

            await client.ListWorkersAsync(WORKSPACE_SID, options);

            mockClient.Verify(trc => trc.Execute<WorkerResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(10, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var activityNameParam = savedRequest.Parameters.Find(x => x.Name == "ActivityName");
            Assert.IsNotNull(activityNameParam);
            Assert.AreEqual(options.ActivityName, activityNameParam.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual(options.ActivitySid, activitySidParam.Value);
            var targetWorkersExpressionParam = savedRequest.Parameters.Find(x => x.Name == "TargetWorkersExpression");
            Assert.IsNotNull(targetWorkersExpressionParam);
            Assert.AreEqual(options.TargetWorkersExpression, targetWorkersExpressionParam.Value);
            var taskQueueNameParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueName");
            Assert.IsNotNull(taskQueueNameParam);
            Assert.AreEqual(options.TaskQueueName, taskQueueNameParam.Value);
            var taskQueueSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskQueueSid");
            Assert.IsNotNull(taskQueueSidParam);
            Assert.AreEqual(options.TaskQueueSid, taskQueueSidParam.Value);
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
        public async System.Threading.Tasks.Task ShouldUpdateWorker()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Worker>();
            tcs.SetResult(new Worker());

            mockClient.Setup(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.UpdateWorkerAsync(WORKSPACE_SID, WORKER_SID, "WA123", "attributes", friendlyName);

            mockClient.Verify(trc => trc.Execute<Worker>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Workers/{WorkerSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workerSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerSid");
            Assert.IsNotNull(workerSidParam);
            Assert.AreEqual(WORKER_SID, workerSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual("WA123", activitySidParam.Value);
            var attributesParam = savedRequest.Parameters.Find(x => x.Name == "Attributes");
            Assert.IsNotNull(attributesParam);
            Assert.AreEqual("attributes", attributesParam.Value);
        }

    }
}

