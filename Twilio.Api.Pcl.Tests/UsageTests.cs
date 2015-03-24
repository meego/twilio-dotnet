using System;
using NUnit.Framework;
using Simple;
using Moq;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class UsageTests
    {
        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldListUsage()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<UsageResult>();
            tcs.SetResult(new UsageResult());

            mockClient.Setup(trc => trc.Execute<UsageResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListUsageAsync("calls", DateTime.Now, DateTime.Now.AddDays(-7));

            mockClient.Verify(trc => trc.Execute<UsageResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Usage/Records.json", savedRequest.Resource);
            Assert.AreEqual(Method.GET, savedRequest.Method);
            //Assert.AreEqual(3, savedRequest.Parameters.Count);
            //var fromParam = savedRequest.Parameters.Find(x => x.Name == "From");
            //Assert.IsNotNull(fromParam);
            //Assert.AreEqual(FROM, fromParam.Value);
            //var toParam = savedRequest.Parameters.Find(x => x.Name == "To");
            //Assert.IsNotNull(toParam);
            //Assert.AreEqual(TO, toParam.Value);
            //var urlParam = savedRequest.Parameters.Find(x => x.Name == "Url");
            //Assert.IsNotNull(urlParam);
            //Assert.AreEqual(URL, urlParam.Value);

        }
    }
}