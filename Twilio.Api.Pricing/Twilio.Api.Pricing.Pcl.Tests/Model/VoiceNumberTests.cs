using System;
using System.IO;
using NUnit.Framework;
using Simple;
using Twilio.Pricing;
using System.Reflection;

namespace Twilio.Pricing.Tests.Model
{
    [TestFixture]
    public class VoiceNumberTests
    {
        private string BASE_NAME = String.Empty;
        private Assembly asm;

        [SetUp]
        public void Setup()
        {
            asm = Assembly.GetExecutingAssembly();
            BASE_NAME = asm.GetName().Name + ".Resources.";
        }

        [Test]
        public void testDeserializeInstanceResponse()
        {
            //var doc = File.ReadAllText(Path.Combine("../../Resources", "voice_number.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "voice_number.json");

            var json = new JsonDeserializer();
            var output = json.Deserialize<VoiceNumber>(new RestResponse { Content = doc });

            Assert.NotNull(output);
            Assert.AreEqual("US", output.IsoCountry);
            Assert.AreEqual("United States", output.Country);
            Assert.AreEqual("USD", output.PriceUnit);
            Assert.AreEqual(0.0075m, output.InboundCallPrice.BasePrice);
            Assert.AreEqual(0.0070m, output.InboundCallPrice.CurrentPrice);
            Assert.AreEqual(0.015m, output.OutboundCallPrice.BasePrice);
            Assert.AreEqual(0.015m, output.OutboundCallPrice.CurrentPrice);
            Assert.AreEqual("+14089673429", output.Number);
        }
    }
}
