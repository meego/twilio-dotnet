using NUnit.Framework;
using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Twilio.Api.Pcl.Tests
{
    [TestFixture]
    public class MetadataListTests
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
        public void Can_Deserialize_Metadata_List_Formated_Objects() 
        {
            var doc = Twilio.Api.Tests.Utilities.UnPack(BASE_NAME + "fake_metadata_list_result.json");
            var json = new JsonDeserializer();
            var output = json.Deserialize<FakeMetaDataListResult>(new RestResponse { Content = doc });

            Assert.NotNull(output);
            Assert.NotNull(output.Meta);
            Assert.AreEqual(output.Meta.Page, 2);
            Assert.AreEqual(output.Meta.PageSize,50);
            Assert.AreEqual(output.Meta.Key, "stuffs");
            Assert.AreEqual(output.Meta.FirstPageUrl, "https://example.com/v1/Stuffs?PageSize=50&Page=0");
            Assert.IsNull(output.Meta.NextPageUrl);
            Assert.AreEqual(output.Meta.PreviousPageUrl, "https://example.com/v1/Stuffs?PageSize=50&Page=1");
            Assert.AreEqual(output.Meta.Url, "https://example.com/v1/Stuffs?PageSize=50&Page=2");
        }
    }
}
