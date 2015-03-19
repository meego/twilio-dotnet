using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Lookups;

namespace Twilio.Api.TaskRouter.Net35.Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void ShouldSetProperBaseUrl()
        {
            var client = new TwilioLookupsClient("XXXXXX", "XXXXXXX");

            Assert.AreEqual("https://lookups.twilio.com/", client.BaseUrl);
        }

        [Test]
        public void ShouldSetProperApiVersion()
        {
            var client = new TwilioLookupsClient("XXXXXX", "XXXXXXX");

            Assert.AreEqual("v1", client.ApiVersion);

        }

    }
}
