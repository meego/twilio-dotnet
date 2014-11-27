using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class UsageTriggerTests
    {
        private const string USAGE_TRIGGER_SID = "UT123";

        //list/get/create/update/delete
        ManualResetEvent manualResetEvent = null;

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetUsageTrigger()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<UsageTrigger>();
            tcs.SetResult(new UsageTrigger());

            mockClient.Setup(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;            
            await client.GetUsageTrigger(USAGE_TRIGGER_SID);

            mockClient.Verify(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Triggers/{UsageTriggerSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var usageTriggerSidParam = savedRequest.Parameters.Find(x => x.Name == "UsageTriggerSid");
            Assert.IsNotNull(usageTriggerSidParam);
            Assert.AreEqual(USAGE_TRIGGER_SID, usageTriggerSidParam.Value);
        }

        [Test]
        public async Task ShouldListUsageTriggers()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<UsageTriggerResult>();
            tcs.SetResult(new UsageTriggerResult());
            
            mockClient.Setup(trc => trc.Execute<UsageTriggerResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListUsageTriggers();

            mockClient.Verify(trc => trc.Execute<UsageTriggerResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Triggers.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldCreateNewUsageTrigger()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<UsageTrigger>();
            tcs.SetResult(new UsageTrigger());
            
            mockClient.Setup(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            UsageTriggerOptions options = new UsageTriggerOptions()
            {
                CallbackUrl = "CallbackUrl",
                TriggerValue = "TriggerValue",
                UsageCategory = "UsageCategory"
            };
            await client.CreateUsageTrigger(options);

            mockClient.Verify(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Triggers.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var callbackUrlParam = savedRequest.Parameters.Find(x => x.Name == "CallbackUrl");
            Assert.IsNotNull(callbackUrlParam);
            Assert.AreEqual(options.CallbackUrl, callbackUrlParam.Value);
            var triggerValueParam = savedRequest.Parameters.Find(x => x.Name == "TriggerValue");
            Assert.IsNotNull(triggerValueParam);
            Assert.AreEqual(options.TriggerValue, triggerValueParam.Value);
            var usageCategoryParam = savedRequest.Parameters.Find(x => x.Name == "UsageCategory");
            Assert.IsNotNull(usageCategoryParam);
            Assert.AreEqual(options.UsageCategory, usageCategoryParam.Value);
        }

        [Test]
        public async Task ShouldUpdateUsageTrigger()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<UsageTrigger>();
            tcs.SetResult(new UsageTrigger());
            
            mockClient.Setup(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.UpdateUsageTrigger(USAGE_TRIGGER_SID, friendlyName, null, null);

            mockClient.Verify(trc => trc.Execute<UsageTrigger>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Triggers/{UsageTriggerSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var usageTriggerSidParam = savedRequest.Parameters.Find(x => x.Name == "UsageTriggerSid");
            Assert.IsNotNull(usageTriggerSidParam);
            Assert.AreEqual(USAGE_TRIGGER_SID, usageTriggerSidParam.Value);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
        }

        [Test]
        public async Task ShouldDeleteUsageTrigger()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());
            
            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.DeleteUsageTrigger(USAGE_TRIGGER_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Triggers/{UsageTriggerSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var usageTriggerSidParam = savedRequest.Parameters.Find(x => x.Name == "UsageTriggerSid");
            Assert.IsNotNull(usageTriggerSidParam);
            Assert.AreEqual(USAGE_TRIGGER_SID, usageTriggerSidParam.Value);
        }

    }
}