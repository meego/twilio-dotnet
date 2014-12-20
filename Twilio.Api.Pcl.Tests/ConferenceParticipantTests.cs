using System;
using NUnit.Framework;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class ConferenceParticipantTests
    {

        private const string CALL_SID = "CA123";

        private const string CONFERENCE_SID = "CF123";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldListConferenceParticipants()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ParticipantResult>();
            tcs.SetResult(new ParticipantResult());

            mockClient.Setup(trc => trc.Execute<ParticipantResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            await client.ListConferenceParticipantsAsync(CONFERENCE_SID, null);

            mockClient.Verify(trc => trc.Execute<ParticipantResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
        }

        [Test]
        public async Task ShouldGetConferenceParticipant()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Participant>();
            tcs.SetResult(new Participant());

            mockClient.Setup(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            await client.GetConferenceParticipantAsync(CONFERENCE_SID, CALL_SID);

            mockClient.Verify(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants/{CallSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
            var callSidParam = savedRequest.Parameters.Find(x => x.Name == "CallSid");
            Assert.IsNotNull(callSidParam);
            Assert.AreEqual(CALL_SID, callSidParam.Value);
        }

        [Test]
        public async Task ShouldMuteConferenceParticipant()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Participant>();
            tcs.SetResult(new Participant());

            mockClient.Setup(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            await client.MuteConferenceParticipantAsync(CONFERENCE_SID, CALL_SID);

            mockClient.Verify(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants/{CallSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
            var callSidParam = savedRequest.Parameters.Find(x => x.Name == "CallSid");
            Assert.IsNotNull(callSidParam);
            Assert.AreEqual(CALL_SID, callSidParam.Value);
            var mutedParam = savedRequest.Parameters.Find(x => x.Name == "Muted");
            Assert.IsNotNull(mutedParam);
            Assert.AreEqual(true, mutedParam.Value);
        }

        [Test]
        public async Task ShouldUnMuteConferenceParticipant()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Participant>();
            tcs.SetResult(new Participant());

            mockClient.Setup(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            await client.UnmuteConferenceParticipantAsync(CONFERENCE_SID, CALL_SID);

            mockClient.Verify(trc => trc.Execute<Participant>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants/{CallSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
            var callSidParam = savedRequest.Parameters.Find(x => x.Name == "CallSid");
            Assert.IsNotNull(callSidParam);
            Assert.AreEqual(CALL_SID, callSidParam.Value);
            var mutedParam = savedRequest.Parameters.Find(x => x.Name == "Muted");
            Assert.IsNotNull(mutedParam);
            Assert.AreEqual(false, mutedParam.Value);
        }

        [Test]
        public async Task ShouldKickConferenceParticipant()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);
            
            var client = mockClient.Object;
            await client.KickConferenceParticipantAsync(CONFERENCE_SID, CALL_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}/Participants/{CallSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
            var callSidParam = savedRequest.Parameters.Find(x => x.Name == "CallSid");
            Assert.IsNotNull(callSidParam);
            Assert.AreEqual(CALL_SID, callSidParam.Value);
        }
    }
}