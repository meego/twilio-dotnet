using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class QueueTests
    {
        private const string QUEUE_SID = "QU123";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Queue>();
            tcs.SetResult(new Queue());

            mockClient.Setup(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetQueueAsync(QUEUE_SID);

            mockClient.Verify(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Queues/{QueueSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var queueSidParam = savedRequest.Parameters.Find(x => x.Name == "QueueSid");
            Assert.IsNotNull(queueSidParam);
            Assert.AreEqual(QUEUE_SID, queueSidParam.Value);
        }

        [Test]
        public async Task ShouldListQueues()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<QueueResult>();
            tcs.SetResult(new QueueResult());
            
            mockClient.Setup(trc => trc.Execute<QueueResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListQueuesAsync();

            mockClient.Verify(trc => trc.Execute<QueueResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Queues.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
     
        [Test]
        public async Task ShouldCreateNewQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Queue>();
            tcs.SetResult(new Queue());
            
            mockClient.Setup(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.CreateQueueAsync(friendlyName);

            mockClient.Verify(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Queues.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
        }

        [Test]
        public async Task ShouldUpdateQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Queue>();
            tcs.SetResult(new Queue());
            
            mockClient.Setup(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.UpdateQueueAsync(QUEUE_SID, friendlyName, 10);

            mockClient.Verify(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Queues/{QueueSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var queueSidParam = savedRequest.Parameters.Find(x => x.Name == "QueueSid");
            Assert.IsNotNull(queueSidParam);
            Assert.AreEqual(QUEUE_SID, queueSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var maxSizeParam = savedRequest.Parameters.Find(x => x.Name == "MaxSize");
            Assert.IsNotNull(maxSizeParam);
            Assert.AreEqual(10, maxSizeParam.Value);
        }

        [Test]
        public async Task ShouldDeleteQueue()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());
            
            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.DeleteQueueAsync(QUEUE_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Queues/{QueueSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var queueSidParam = savedRequest.Parameters.Find(x => x.Name == "QueueSid");
            Assert.IsNotNull(queueSidParam);
            Assert.AreEqual(QUEUE_SID, queueSidParam.Value);
        }
    }
}