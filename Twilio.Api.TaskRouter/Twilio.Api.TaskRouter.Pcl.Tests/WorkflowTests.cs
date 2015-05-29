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
    public class WorkflowTests
    {
        private const string WORKFLOW_SID = "WF123";

        private const string WORKSPACE_SID = "WS123";

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldAddNewWorkflow()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workflow>();
            tcs.SetResult(new Workflow());

            mockClient.Setup(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.AddWorkflowAsync(WORKSPACE_SID, friendlyName, "configuration", "http://www.example.com/assignment", "http://www.example.com/fallback", 60);

            mockClient.Verify(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(6, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentCallbackUrl");
            Assert.IsNotNull(assignmentCallbackUrlParam);
            Assert.AreEqual("http://www.example.com/assignment", assignmentCallbackUrlParam.Value);
            var fallbackAssignmentCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "FallbackAssignmentCallbackUrl");
            Assert.IsNotNull(fallbackAssignmentCallbackUrlParam);
            Assert.AreEqual("http://www.example.com/fallback", fallbackAssignmentCallbackUrlParam.Value);
            var taskReservationTimeoutParam = savedRequest.Parameters.Find(x => x.Name == "TaskReservationTimeout");
            Assert.IsNotNull(taskReservationTimeoutParam);
            Assert.AreEqual(60, taskReservationTimeoutParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldDeleteWorkflow()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.DeleteWorkflowAsync(WORKSPACE_SID, WORKFLOW_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows/{WorkflowSid}", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSid = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSid);
            Assert.AreEqual(WORKSPACE_SID, workspaceSid.Value);
            var workflowSid = savedRequest.Parameters.Find(x => x.Name == "WorkflowSid");
            Assert.IsNotNull(workflowSid);
            Assert.AreEqual(WORKFLOW_SID, workflowSid.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldGetWorkflow()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workflow>();
            tcs.SetResult(new Workflow());

            mockClient.Setup(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetWorkflowAsync(WORKSPACE_SID, WORKFLOW_SID);

            mockClient.Verify(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows/{WorkflowSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workflowSid = savedRequest.Parameters.Find(x => x.Name == "WorkflowSid");
            Assert.IsNotNull(workflowSid);
            Assert.AreEqual(WORKFLOW_SID, workflowSid.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkflows()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkflowResult>();
            tcs.SetResult(new WorkflowResult());

            mockClient.Setup(trc => trc.Execute<WorkflowResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListWorkflowsAsync(WORKSPACE_SID);

            mockClient.Verify(trc => trc.Execute<WorkflowResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldListWorkflowsUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkflowResult>();
            tcs.SetResult(new WorkflowResult());

            mockClient.Setup(trc => trc.Execute<WorkflowResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.ListWorkflowsAsync(WORKSPACE_SID, friendlyName, "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<WorkflowResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
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
        public async System.Threading.Tasks.Task ShouldUpdateWorkflow()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Workflow>();
            tcs.SetResult(new Workflow());

            mockClient.Setup(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var friendlyName = Twilio.Api.Tests.Utilities.MakeRandomFriendlyName();

            await client.UpdateWorkflowAsync(WORKSPACE_SID, WORKFLOW_SID, friendlyName, "http://www.example.com/assignment", "http://www.example.com/fallback", "configuration", 60);

            mockClient.Verify(trc => trc.Execute<Workflow>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows/{WorkflowSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workflowSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkflowSid");
            Assert.IsNotNull(workflowSidParam);
            Assert.AreEqual(WORKFLOW_SID, workflowSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var assignmentCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentCallbackUrl");
            Assert.IsNotNull(assignmentCallbackUrlParam);
            Assert.AreEqual("http://www.example.com/assignment", assignmentCallbackUrlParam.Value);
            var fallbackAssignmentCallbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "FallbackAssignmentCallbackUrl");
            Assert.IsNotNull(fallbackAssignmentCallbackUrlParam);
            Assert.AreEqual("http://www.example.com/fallback", fallbackAssignmentCallbackUrlParam.Value);
            var taskReservationTimeoutParam = savedRequest.Parameters.Find(x => x.Name == "TaskReservationTimeout");
            Assert.IsNotNull(taskReservationTimeoutParam);
            Assert.AreEqual(60, taskReservationTimeoutParam.Value);
        }
    }
}

