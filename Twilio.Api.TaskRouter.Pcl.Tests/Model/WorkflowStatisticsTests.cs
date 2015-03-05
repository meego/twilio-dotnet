using System;
using System.IO;
using NUnit.Framework;
using Twilio.TaskRouter;
using System.Reflection;
using Simple;

namespace Twilio.TaskRouter.Tests.Integration.Model
{
    [TestFixture]
    public class WorkflowStatisticsTests
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
        public void testDeserializeResponse()
        {
            //var doc = File.ReadAllText(Path.Combine("Resources", "workflow_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "workflow_statistics.json");
            var json = new JsonDeserializer();
            var output = json.Deserialize<WorkflowStatistics>(new RestResponse { Content = doc });

            Assert.NotNull(output);
        }
    }
}

