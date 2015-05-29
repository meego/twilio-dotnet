using System;
using System.Threading;
using Moq;
using NUnit.Framework;
//using RestSharp;
//using RestSharp.Deserializers;
using Simple;

using Twilio.TaskRouter;
using Twilio.Api.Tests;
using System.Threading.Tasks;

namespace Twilio.TaskRouter.Tests
{
    [TestFixture]
    public class WorkflowStatisticsTests
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
        public async System.Threading.Tasks.Task ShouldGetWorkflowStatistics()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<WorkflowStatistics>();
            tcs.SetResult(new WorkflowStatistics());

            mockClient.Setup(trc => trc.Execute<WorkflowStatistics>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var options = new StatisticsRequest();
            options.Minutes = 10;

            await client.GetWorkflowStatisticsAsync(WORKSPACE_SID, WORKFLOW_SID, options);

            mockClient.Verify(trc => trc.Execute<WorkflowStatistics>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Workflows/{WorkflowSid}/Statistics", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var workflowSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkflowSid");
            Assert.IsNotNull(workflowSidParam);
            Assert.AreEqual(WORKFLOW_SID, workflowSidParam.Value);
            var minutesParam = savedRequest.Parameters.Find(x => x.Name == "Minutes");
            Assert.IsNotNull(minutesParam);
            Assert.AreEqual(10, minutesParam.Value);
        }

    }
}

