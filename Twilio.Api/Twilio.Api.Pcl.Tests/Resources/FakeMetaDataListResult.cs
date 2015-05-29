using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twilio.Api.Pcl.Tests
{
    public class FakeMetaDataListResult : MetadataListBase
    {
        public List<Stuff> Stuffs { get; set; }
    }

    public class Stuff {
        public string Name {get;set;}
    }
}
