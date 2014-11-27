using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class OutgoingCallerIdTests
    {
        private const string OUTGOING_CALLER_ID_SID = "PN123";

        private const string PHONE_NUMBER = "+15005550006";

        ManualResetEvent manualResetEvent = null;

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldAddNewOutgoingCallerId()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ValidationRequestResult>();
            tcs.SetResult(new ValidationRequestResult());
            
            mockClient.Setup(trc => trc.Execute<ValidationRequestResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.AddOutgoingCallerId(PHONE_NUMBER, friendlyName, null, null);

            mockClient.Verify(trc => trc.Execute<ValidationRequestResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var PhoneNumberParam = savedRequest.Parameters.Find(x => x.Name == "PhoneNumber");
            Assert.IsNotNull(PhoneNumberParam);
            Assert.AreEqual(PHONE_NUMBER, PhoneNumberParam.Value);
        }

        [Test]
        public async Task ShouldListOutgoingCallerIds()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<OutgoingCallerIdResult>();
            tcs.SetResult(new OutgoingCallerIdResult());
            
            mockClient.Setup(trc => trc.Execute<OutgoingCallerIdResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListOutgoingCallerIds();

            mockClient.Verify(trc => trc.Execute<OutgoingCallerIdResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListOutgoingCallerIdsUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<OutgoingCallerIdResult>();
            tcs.SetResult(new OutgoingCallerIdResult());
            
            mockClient.Setup(trc => trc.Execute<OutgoingCallerIdResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.ListOutgoingCallerIds(PHONE_NUMBER, friendlyName, null, null);

            mockClient.Verify(trc => trc.Execute<OutgoingCallerIdResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var PhoneNumberParam = savedRequest.Parameters.Find(x => x.Name == "PhoneNumber");
            Assert.IsNotNull(PhoneNumberParam);
            Assert.AreEqual(PHONE_NUMBER, PhoneNumberParam.Value);
        }

        [Test]
        public async Task ShouldGetOutgoingCallerId()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<OutgoingCallerId>();
            tcs.SetResult(new OutgoingCallerId());
            
            mockClient.Setup(trc => trc.Execute<OutgoingCallerId>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetOutgoingCallerId(OUTGOING_CALLER_ID_SID);

            mockClient.Verify(trc => trc.Execute<OutgoingCallerId>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var outgoingCallerIdSidParam = savedRequest.Parameters.Find(x => x.Name == "OutgoingCallerIdSid");
            Assert.IsNotNull(outgoingCallerIdSidParam);
            Assert.AreEqual(OUTGOING_CALLER_ID_SID, outgoingCallerIdSidParam.Value);
        }

        [Test]
        public async Task ShouldUpdateOutgoingCallerId()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<OutgoingCallerId>();
            tcs.SetResult(new OutgoingCallerId());
            
            mockClient.Setup(trc => trc.Execute<OutgoingCallerId>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            await client.UpdateOutgoingCallerIdName(OUTGOING_CALLER_ID_SID, friendlyName);

            mockClient.Verify(trc => trc.Execute<OutgoingCallerId>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);
            var outgoingCallerIdSidParam = savedRequest.Parameters.Find(x => x.Name == "OutgoingCallerIdSid");
            Assert.IsNotNull(outgoingCallerIdSidParam);
            Assert.AreEqual(OUTGOING_CALLER_ID_SID, outgoingCallerIdSidParam.Value);
        }

        [Test]
        public async Task ShouldDeleteOutgoingCallerId()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());
            
            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.DeleteOutgoingCallerId(OUTGOING_CALLER_ID_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/OutgoingCallerIds/{OutgoingCallerIdSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var outgoingCallerIdSidParam = savedRequest.Parameters.Find(x => x.Name == "OutgoingCallerIdSid");
            Assert.IsNotNull(outgoingCallerIdSidParam);
            Assert.AreEqual(OUTGOING_CALLER_ID_SID, outgoingCallerIdSidParam.Value);
        }
    }
}