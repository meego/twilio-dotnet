using Moq;
using NUnit.Framework;
using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class TokenTests
    {
        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldCreateToken()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Token>();
            tcs.SetResult(new Token());

            mockClient.Setup(trc => trc.Execute<Token>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.CreateTokenAsync();

            mockClient.Verify(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Tokens.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
        }

        [Test]
        public async Task ShouldCreateTokenWithTtl()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<Token>();
            tcs.SetResult(new Token());

            mockClient.Setup(trc => trc.Execute<Token>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var ttl = 30;
            await client.CreateTokenAsync(ttl);

            mockClient.Verify(trc => trc.Execute<Queue>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/Tokens.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var ttlParam = savedRequest.Parameters.Find(x => x.Name == "Ttl");
            Assert.IsNotNull(ttlParam);
            Assert.AreEqual(ttl, ttlParam.Value);
        }
    }
}
