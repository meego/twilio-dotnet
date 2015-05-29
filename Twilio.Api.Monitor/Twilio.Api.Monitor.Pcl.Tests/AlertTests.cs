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
    public class AlertTests
    {
        private const string ALERT_SID = "AE123";

        private Mock<MonitorClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<MonitorClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetAlert()
        {
            var tcs = new TaskCompletionSource<Alert>();
            tcs.SetResult(new Alert());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Alert>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.GetAlertAsync(ALERT_SID);

            mockClient.Verify(trc => trc.Execute<Alert>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Alerts/{AlertSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var alertSidParam = savedRequest.Parameters.Find(x => x.Name == "AlertSid");
            Assert.IsNotNull(alertSidParam);
            Assert.AreEqual(ALERT_SID, alertSidParam.Value);
        }

        [Test]
        public async Task ShouldListAlerts()
        {
            var tcs = new TaskCompletionSource<AlertResult>();
            tcs.SetResult(new AlertResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<AlertResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;

            await client.ListAlertsAsync();

            mockClient.Verify(trc => trc.Execute<AlertResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Alerts", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListAlertsUsingFilters()
        {
            var tcs = new TaskCompletionSource<AlertResult>();
            tcs.SetResult(new AlertResult());

            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<AlertResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            var client = mockClient.Object;
            var date = DateTime.UtcNow;

            await client.ListAlertsAsync("error", date, date);

            mockClient.Verify(trc => trc.Execute<AlertResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Alerts", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var startDateNameParam = savedRequest.Parameters.Find(x => x.Name == "StartDate");
            Assert.IsNotNull(startDateNameParam);
            Assert.AreEqual(date.ToString("yyyy-MM-ddTHH:mm:ssK"), startDateNameParam.Value);
            var endDateNameParam = savedRequest.Parameters.Find(x => x.Name == "EndDate");
            Assert.IsNotNull(endDateNameParam);
            Assert.AreEqual(date.ToString("yyyy-MM-ddTHH:mm:ssK"), endDateNameParam.Value);
            var logLevel = savedRequest.Parameters.Find(x => x.Name == "LogLevel");
            Assert.IsNotNull(logLevel);
            Assert.AreEqual("error", logLevel.Value);
        }
      
    }
}

