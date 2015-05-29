using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using Simple;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class TranscriptionTests
    {
        private const string TRANSCRIPTION_SID = "";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetTranscription()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Transcription>();
            tcs.SetResult(new Transcription());

            mockClient.Setup(trc => trc.Execute<Transcription>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetTranscriptionAsync(TRANSCRIPTION_SID);

            mockClient.Verify(trc => trc.Execute<Transcription>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Transcriptions/{TranscriptionSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var transcriptionSidParam = savedRequest.Parameters.Find(x => x.Name == "TranscriptionSid");
            Assert.IsNotNull(transcriptionSidParam);
            Assert.AreEqual(TRANSCRIPTION_SID, transcriptionSidParam.Value);
        }

        [Test]
        public async Task ShouldListTranscriptions()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TranscriptionResult>();
            tcs.SetResult(new TranscriptionResult());

            mockClient.Setup(trc => trc.Execute<TranscriptionResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListTranscriptionsAsync();

            mockClient.Verify(trc => trc.Execute<TranscriptionResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Transcriptions.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldListTranscriptionsUsingFilters()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<TranscriptionResult>();
            tcs.SetResult(new TranscriptionResult());

            mockClient.Setup(trc => trc.Execute<TranscriptionResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);


            var client = mockClient.Object;
            string recordingSid = "";
            await client.ListTranscriptionsAsync(recordingSid, 0, 10);

            mockClient.Verify(trc => trc.Execute<TranscriptionResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Recordings/{RecordingSid}/Transcriptions.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var recordingSidParam = savedRequest.Parameters.Find(x => x.Name == "RecordingSid");
            Assert.IsNotNull(recordingSidParam);
            Assert.AreEqual(recordingSid, recordingSidParam.Value);
            var pageNumberParam = savedRequest.Parameters.Find(x => x.Name == "Page");
            Assert.IsNotNull(pageNumberParam);
            Assert.AreEqual(0, pageNumberParam.Value);
            var countParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countParam);
            Assert.AreEqual(10, countParam.Value);
        }

        [Test]
        public async Task ShouldDeleteTranscription()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<RestResponse>();
            tcs.SetResult(new RestResponse());

            mockClient.Setup(trc => trc.Execute(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.DeleteTranscriptionAsync(TRANSCRIPTION_SID);

            mockClient.Verify(trc => trc.Execute(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Transcriptions/{TranscriptionSid}.json", savedRequest.Resource);
            Assert.AreEqual("DELETE", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var transcriptionSidParam = savedRequest.Parameters.Find(x => x.Name == "TranscriptionSid");
            Assert.IsNotNull(transcriptionSidParam);
            Assert.AreEqual(TRANSCRIPTION_SID, transcriptionSidParam.Value);
        }

    }
}