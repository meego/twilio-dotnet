using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRestClient.Tests
{
    public class FakeHttpWebResponse : HttpWebResponse
    {
		private readonly WebHeaderCollection _headers;
        private static SerializationInfo SerializationInfo;
        private readonly MemoryStream _responseStream;

        public static void InitializeHttpWebResponse(HttpStatusCode status, string statusDescription)
        {
            InitializeHttpWebResponse(status, statusDescription, 0);
        }

        public static void InitializeHttpWebResponse(HttpStatusCode status, string statusDescription, int contentLength)
        {
            SerializationInfo = new SerializationInfo(typeof(HttpWebResponse), new FormatterConverter());
            //StreamingContext sc = new StreamingContext();
            WebHeaderCollection headers = new WebHeaderCollection();
            SerializationInfo.AddValue("m_HttpResponseHeaders", headers);
            SerializationInfo.AddValue("uri", new Uri("http://example.com"));
            SerializationInfo.AddValue("m_Certificate", null);
            SerializationInfo.AddValue("version", HttpVersion.Version11);
            SerializationInfo.AddValue("statusCode", status);
            SerializationInfo.AddValue("contentLength", contentLength);
			SerializationInfo.AddValue("contentType", "");
			SerializationInfo.AddValue("method", "GET");
            SerializationInfo.AddValue("m_Verb", "GET");
            SerializationInfo.AddValue("statusDescription", statusDescription);
            SerializationInfo.AddValue("m_MediaType", null);
            SerializationInfo.AddValue("m_ConnectStream", null, typeof(Stream));
			SerializationInfo.AddValue("cookieCollection", null, typeof(CookieCollection));
        }

#pragma warning disable 618
        public FakeHttpWebResponse()
            : base(SerializationInfo, new StreamingContext())
        {

        }
#pragma warning restore 618

        public FakeHttpWebResponse(Stream response)
            : this()
        {
			_headers = new WebHeaderCollection();
            _responseStream = (MemoryStream)response;

        }

        public override Stream GetResponseStream()
        {
            return this._responseStream;
        }

		public override WebHeaderCollection Headers {
			get {
				return this._headers;
			}
		}
    }
}
