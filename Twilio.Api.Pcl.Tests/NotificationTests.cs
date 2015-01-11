using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class NotificationTests
    {
        private const string NOTIFICATION_SID = "";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetNotification()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Notification>();
            tcs.SetResult(new Notification());

            mockClient.Setup(trc => trc.Execute<Notification>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetNotificationAsync(NOTIFICATION_SID);

            mockClient.Verify(trc => trc.Execute<Notification>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Notifications/{NotificationSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var notificationSidParam = savedRequest.Parameters.Find(x => x.Name == "NotificationSid");
            Assert.IsNotNull(notificationSidParam);
            Assert.AreEqual(NOTIFICATION_SID, notificationSidParam.Value);
        }

        [Test]
        public async Task ShouldListNotification()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<NotificationResult>();
            tcs.SetResult(new NotificationResult());

            mockClient.Setup(trc => trc.Execute<NotificationResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListNotificationsAsync();

            mockClient.Verify(trc => trc.Execute<NotificationResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Notifications.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListNotificationUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<NotificationResult>();
            tcs.SetResult(new NotificationResult());

            mockClient.Setup(trc => trc.Execute<NotificationResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListNotificationsAsync(0, null, null, null);

            mockClient.Verify(trc => trc.Execute<NotificationResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Notifications.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var logParam = savedRequest.Parameters.Find(x => x.Name == "Log");
            Assert.IsNotNull(logParam);
            Assert.AreEqual(0, logParam.Value);
        }

        [Test]
        public async Task ShouldDeleteNotification()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.DeleteNotificationAsync(NOTIFICATION_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Notifications/{NotificationSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var notificationSidParam = savedRequest.Parameters.Find(x => x.Name == "NotificationSid");
            Assert.IsNotNull(notificationSidParam);
            Assert.AreEqual(NOTIFICATION_SID, notificationSidParam.Value);
        }
    }
}