using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Simple
{
    public partial class RestClient
    {
        /// <summary>
        /// create instance by default if none exists
        /// </summary>
        private HttpMessageHandler _messageHandler;
        public HttpMessageHandler MessageHandler
        {
            get
            {
                return _messageHandler;
            }
            set
            {
                _messageHandler = value;
            }
        }
    }
}
