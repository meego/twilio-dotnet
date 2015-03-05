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
    public class ActivityTests
    {
        private const string ACTIVITY_SID = "WA123";

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
        public async System.Threading.Tasks.Task ShouldAddNewActivity()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Activity>();
            tcs.SetResult(new Activity());

            mockClient.Setup(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddActivityAsync(WORKSPACE_SID, friendlyName, true);

            mockClient.Verify(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var availableParam = savedRequest.Parameters.Find(x => x.Name == "Available");
            Assert.IsNotNull(availableParam);
            Assert.AreEqual(true, availableParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteActivity()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteActivityAsync(WORKSPACE_SID, ACTIVITY_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual(ACTIVITY_SID, activitySidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldGetActivity()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Activity>();
            tcs.SetResult(new Activity());

            mockClient.Setup(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetActivityAsync(WORKSPACE_SID, ACTIVITY_SID);

            mockClient.Verify(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual(ACTIVITY_SID, activitySidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListActivities()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ActivityResult>();
            tcs.SetResult(new ActivityResult());

            mockClient.Setup(trc => trc.Execute<ActivityResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListActivitiesAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<ActivityResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListActivitiesUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ActivityResult>();
            tcs.SetResult(new ActivityResult());

            mockClient.Setup(trc => trc.Execute<ActivityResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.ListActivitiesAsync(WORKSPACE_SID, true, friendlyName, "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<ActivityResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var availableParam = savedRequest.Parameters.Find(x => x.Name == "Available");
            Assert.IsNotNull(availableParam);
            Assert.AreEqual(true, availableParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
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
        public async System.Threading.Tasks.Task ShouldUpdateActivity()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Activity>();
            tcs.SetResult(new Activity());

            mockClient.Setup(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.UpdateActivityAsync(WORKSPACE_SID, ACTIVITY_SID, friendlyName, true);

            mockClient.Verify(trc => trc.Execute<Activity>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}/Activities/{ActivitySid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var activitySidParam = savedRequest.Parameters.Find(x => x.Name == "ActivitySid");
            Assert.IsNotNull(activitySidParam);
            Assert.AreEqual(ACTIVITY_SID, activitySidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var availableParam = savedRequest.Parameters.Find(x => x.Name == "Available");
            Assert.IsNotNull(availableParam);
            Assert.AreEqual(true, availableParam.Value);
        }

    }
}

