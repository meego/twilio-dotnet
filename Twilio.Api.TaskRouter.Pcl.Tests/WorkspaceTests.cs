using System;
using System.Threading;
using Moq;
using NUnit.Framework;
//using Simple;

using Twilio.Api.Tests;
using Simple;
using System.Threading.Tasks;
//using Twilio.Api.Tests.Integration;

namespace Twilio.TaskRouter.Tests
{
    [TestFixture]
    public class WorkspaceTests
    {
        private const string WORKSPACE_SID = "WS123";

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldAddNewWorkspace()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workspace>();
            tcs.SetResult(new Workspace());

            mockClient.Setup(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddWorkspaceAsync(friendlyName, "http://www.example.com", "template");

            mockClient.Verify(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var eventCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "EventCallbackUrl");
            Assert.IsNotNull(eventCallbackUrlParam);
            Assert.AreEqual("http://www.example.com", eventCallbackUrlParam.Value);
            var templateParam = savedRequest.Parameters.Find(x => x.Name == "Template");
            Assert.IsNotNull(templateParam);
            Assert.AreEqual("template", templateParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteWorkspace()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteWorkspaceAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldGetWorkspace()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workspace>();
            tcs.SetResult(new Workspace());

            mockClient.Setup(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetWorkspaceAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkspaces()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkspaceResult>();
            tcs.SetResult(new WorkspaceResult());

            mockClient.Setup(trc => trc.Execute<WorkspaceResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListWorkspacesAsync();

            mockClient.Verify(trc => trc.Execute<WorkspaceResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkspacesUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkspaceResult>();
            tcs.SetResult(new WorkspaceResult());

            mockClient.Setup(trc => trc.Execute<WorkspaceResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.ListWorkspacesAsync(friendlyName, "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<WorkspaceResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(4, savedRequest.Parameters.Count);
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
        public async System.Threading.Tasks.Task ShouldUpdateWorkspace()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workspace>();
            tcs.SetResult(new Workspace());

            mockClient.Setup(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.UpdateWorkspaceAsync(WORKSPACE_SID, friendlyName, "http://www.example.com", "template", "WA123", "WA234");

            mockClient.Verify(trc => trc.Execute<Workspace>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Workspaces/{WorkspaceSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var eventCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "EventCallbackUrl");
            Assert.IsNotNull(eventCallbackUrlParam);
            Assert.AreEqual("http://www.example.com", eventCallbackUrlParam.Value);
            var defaultActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "DefaultActivitySid");
            Assert.IsNotNull(defaultActivitySidParam);
            Assert.AreEqual("WA123", defaultActivitySidParam.Value);
            var timeoutActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "TimeoutActivitySid");
            Assert.IsNotNull(timeoutActivitySidParam);
            Assert.AreEqual("WA234", timeoutActivitySidParam.Value);
        }
    }
}

