using System;
using NUnit.Framework;
using System.Threading;
using Moq;
using Simple;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class ConferenceTests
    {
        private const string CONFERENCE_SID = "CF123";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetConference()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Conference>();
            tcs.SetResult(new Conference());

            mockClient.Setup(trc => trc.Execute<Conference>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetConferenceAsync(CONFERENCE_SID);

            mockClient.Verify(trc => trc.Execute<Conference>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences/{ConferenceSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var conferenceSidParam = savedRequest.Parameters.Find(x => x.Name == "ConferenceSid");
            Assert.IsNotNull(conferenceSidParam);
            Assert.AreEqual(CONFERENCE_SID, conferenceSidParam.Value);
        }

        [Test]
        public async Task ShouldListConferences()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ConferenceResult>();
            tcs.SetResult(new ConferenceResult());

            mockClient.Setup(trc => trc.Execute<ConferenceResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListConferencesAsync();

            mockClient.Verify(trc => trc.Execute<ConferenceResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
     
        [Test]
        public async Task ShouldListConferenceUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ConferenceResult>();
            tcs.SetResult(new ConferenceResult());

            mockClient.Setup(trc => trc.Execute<ConferenceResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            ConferenceListRequest options = new ConferenceListRequest();
            await client.ListConferencesAsync(options);

            mockClient.Verify(trc => trc.Execute<ConferenceResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Conferences.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }
    }
}