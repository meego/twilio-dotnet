using System;
using System.IO;
using NUnit.Framework;
using Twilio.TaskRouter;
using System.Reflection;
using Simple;

namespace Twilio.TaskRouter.Tests.Integration.Model
{
    [TestFixture]
    public class WorkerStatisticsTests
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
            //var doc = File.ReadAllText(Path.Combine("Resources", "worker_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "worker_statistics.json");
            var json = new JsonDeserializer();
            var output = json.Deserialize<WorkerStatistics>(new RestResponse { Content = doc });

            Assert.NotNull(output);
        }

        [Test]
        public void testDeserializeListResponse()
        {
            //var doc = File.ReadAllText(Path.Combine("Resources", "workers_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "workers_statistics.json");
            var json = new JsonDeserializer();
            var output = json.Deserialize<WorkersStatistics>(new RestResponse { Content = doc });

            Assert.NotNull(output);
        }
    }
}


