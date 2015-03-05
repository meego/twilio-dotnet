using System;
using System.IO;
using NUnit.Framework;
//using RestSharp;
//using RestSharp.Deserializers;
using Simple;
using Twilio.TaskRouter;
using System.Reflection;

namespace Twilio.TaskRouter.Tests.Integration.Model
{
    [TestFixture]
    public class WorkspaceStatisticsTests
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
            //var doc = File.ReadAllText(Path.Combine("Resources", "workspace_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "workspace_statistics.json");
            var json = new JsonDeserializer();
            var output = json.Deserialize<WorkspaceStatistics>(new RestResponse { Content = doc });

            Assert.NotNull(output);
        }
    }
}

