using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TaskRouter;

namespace Twilio.Api.TaskRouter.Pcl.Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void ShouldSetProperBaseUrl()
        {
            var client = new TaskRouterClient("XXXXXX", "XXXXXXX");

            Assert.AreEqual("https://taskrouter.twilio.com/", client.BaseUrl);
        }

        [Test]
        public void ShouldSetProperApiVersion()
        {
            var client = new TaskRouterClient("XXXXXX", "XXXXXXX");

            Assert.AreEqual("v1", client.ApiVersion);

        }

    }
}
