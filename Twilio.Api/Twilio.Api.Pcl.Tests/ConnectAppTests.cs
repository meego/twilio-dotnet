using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Simple;
using Moq;

namespace Twilio.Api.Tests
{
    [TestFixture]
    public class ConnectAppTests
    {
        private const string CONNECTAPP_SID = "CN123";

        private Mock<TwilioRestClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TwilioRestClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public async Task ShouldGetConnectApp()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ConnectApp>();
            tcs.SetResult(new ConnectApp());

            mockClient.Setup(trc => trc.Execute<ConnectApp>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.GetConnectAppAsync(CONNECTAPP_SID);

            mockClient.Verify(trc => trc.Execute<ConnectApp>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/ConnectApps/{ConnectAppSid}.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(1, savedRequest.Parameters.Count);
            var connectappSidParam = savedRequest.Parameters.Find(x => x.Name == "ConnectAppSid");
            Assert.IsNotNull(connectappSidParam);
            Assert.AreEqual(CONNECTAPP_SID, connectappSidParam.Value);
        }

        [Test]
        public async Task ShouldListConnectApps()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ConnectAppResult>();
            tcs.SetResult(new ConnectAppResult());

            mockClient.Setup(trc => trc.Execute<ConnectAppResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            await client.ListConnectAppsAsync();

            mockClient.Verify(trc => trc.Execute<ConnectAppResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/ConnectApps.json", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(0, savedRequest.Parameters.Count);
        }

        [Test]
        public async Task ShouldUpdateConnectApp()
        {
            RestRequest savedRequest = null;

            var tcs = new TaskCompletionSource<ConnectApp>();
            tcs.SetResult(new ConnectApp());

            mockClient.Setup(trc => trc.Execute<ConnectApp>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(tcs.Task);

            var client = mockClient.Object;
            var friendlyName = Utilities.MakeRandomFriendlyName();
            var authorizeUrl = "http://example.com/authorize";
            var deauthorizeUrl = "http://example.com/deauthorize";
            
            await client.UpdateConnectAppAsync(CONNECTAPP_SID, friendlyName, authorizeUrl, deauthorizeUrl, "GET", "", "", "", "");

            mockClient.Verify(trc => trc.Execute<ConnectApp>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Accounts/{AccountSid}/ConnectApps/{ConnectAppSid}.json", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);

            var connectAppSidParam = savedRequest.Parameters.Find(x => x.Name == "ConnectAppSid");
            Assert.IsNotNull(connectAppSidParam);
            Assert.AreEqual(CONNECTAPP_SID, connectAppSidParam.Value);

            var friendlyNameParam = savedRequest.Parameters.Find(x => x.Name == "FriendlyName");
            Assert.IsNotNull(friendlyNameParam);
            Assert.AreEqual(friendlyName, friendlyNameParam.Value);

            var authUrlParam = savedRequest.Parameters.Find(x => x.Name == "AuthorizeRedirectUrl");
            Assert.IsNotNull(authUrlParam);
            Assert.AreEqual(authorizeUrl, authUrlParam.Value);

            var deauthUrlParam = savedRequest.Parameters.Find(x => x.Name == "DeauthorizeCallbackUrl");
            Assert.IsNotNull(deauthUrlParam);
            Assert.AreEqual(deauthorizeUrl, deauthUrlParam.Value);

            var methodParam = savedRequest.Parameters.Find(x => x.Name == "DeauthorizeCallbackMethod");
            Assert.IsNotNull(methodParam);
            Assert.AreEqual("GET", methodParam.Value);
        }

    }
}