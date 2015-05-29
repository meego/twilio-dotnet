using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Simple;
using Twilio.Api.Tests;
using System.Threading.Tasks;

namespace Twilio.Monitor.Tests
{
    [TestFixture]
    public class EventTests
    {
        private const string EVENT_SID = "AE123";

        private Mock<MonitorClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<MonitorClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetEvent()
        {
            var tcs = new TaskCompletionSource<Event>();
            tcs.SetResult(new Event());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Event>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetEventAsync(EVENT_SID);

            mockClient.Verify(trc => trc.Execute<Event>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Events/{EventSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var eventSidParam = savedRequest.Parameters.Find(x => x.Name == "EventSid");
            Assert.IsNotNull(eventSidParam);
            Assert.AreEqual(EVENT_SID, eventSidParam.Value);
        }

        [Test]
        public async Task ShouldListEvents()
        {
            var tcs = new TaskCompletionSource<EventResult>();
            tcs.SetResult(new EventResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<EventResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListEventsAsync();

            mockClient.Verify(trc => trc.Execute<EventResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Events", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListEventsUsingFilters()
        {
            var tcs = new TaskCompletionSource<EventResult>();
            tcs.SetResult(new EventResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<EventResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var options = new EventListRequest();
            options.ActorSid = "AC123";
            options.StartDate = DateTime.UtcNow;
            options.EndDate = DateTime.UtcNow;
            options.ResourceSid = "WK123";
            options.EventType = "update";
            options.Count = 10;
            options.PageToken = "pageToken";

            await client.ListEventsAsync(options);

            mockClient.Verify(trc => trc.Execute<EventResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Events", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var actorSidNameParam = savedRequest.Parameters.Find(x => x.Name == "ActorSid");
            Assert.IsNotNull(actorSidNameParam);
            Assert.AreEqual(options.ActorSid, actorSidNameParam.Value);
            var startDateNameParam = savedRequest.Parameters.Find(x => x.Name == "StartDate");
            Assert.IsNotNull(startDateNameParam);
            Assert.AreEqual(options.StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"), startDateNameParam.Value);
            var endDateNameParam = savedRequest.Parameters.Find(x => x.Name == "EndDate");
            Assert.IsNotNull(endDateNameParam);
            Assert.AreEqual(options.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"), endDateNameParam.Value);
            var ResourceSidNameParam = savedRequest.Parameters.Find(x => x.Name == "ResourceSid");
            Assert.IsNotNull(ResourceSidNameParam);
            Assert.AreEqual(options.ResourceSid, ResourceSidNameParam.Value);
            var eventTypeNameParam = savedRequest.Parameters.Find(x => x.Name == "EventType");
            Assert.IsNotNull(eventTypeNameParam);
            Assert.AreEqual(options.EventType, eventTypeNameParam.Value);
            var countNameParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countNameParam);
            Assert.AreEqual(options.Count, countNameParam.Value);
            var pageTokenNameParam = savedRequest.Parameters.Find(x => x.Name == "PageToken");
            Assert.IsNotNull(pageTokenNameParam);
            Assert.AreEqual(options.PageToken, pageTokenNameParam.Value);
        }
    }
}

