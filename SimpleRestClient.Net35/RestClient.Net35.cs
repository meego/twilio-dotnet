using Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple
{
    public partial class RestClient
    {
        /// <summary>
        /// create instance by default if none exists
        /// </summary>
        private HttpWebRequestWrapper _requestwrapper;

        /// <summary>
        /// HttpWebRequestWrapper to use for making requests
        /// </summary>
        public HttpWebRequestWrapper WebRequest
        {
            get
            {
                if (_requestwrapper == null)
                {
                    _requestwrapper = new HttpWebRequestWrapper();
                }
                return _requestwrapper;
            }
            set
            {
                _requestwrapper = value;
            }
        }


    }
}
