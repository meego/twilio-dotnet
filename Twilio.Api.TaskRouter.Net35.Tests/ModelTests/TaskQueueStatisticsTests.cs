﻿using System;
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
    public class TaskQueueStatisticsTests
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
            //var doc = File.ReadAllText(Path.Combine("Resources", "task_queue_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "task_queue_statistics.json");

            var json = new JsonDeserializer();
            var output = json.Deserialize<TaskQueueStatistics>(new RestResponse { Content = doc });

            Assert.NotNull(output);
        }

        [Test]
        public void testDeserializeListResponse()
        {
            //var doc = File.ReadAllText(Path.Combine("Resources", "task_queues_statistics.json"));
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "task_queues_statistics.json");

            var json = new JsonDeserializer();
            var output = json.Deserialize<TaskQueueStatisticsResult>(new RestResponse { Content = doc });

            Assert.NotNull(output);
            Assert.NotNull(output.Meta);
        }
    }
}

