using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Threading;

namespace Simple
{
    /// <summary>
    /// A simple class for making requests to HTTP API's
    /// </summary>
    public partial class RestClient
    {

        HttpClient _instance;
        
        /// <summary>
        /// Singleton instance of HTTP Client.  This allows for reusing the client across multiple requests.
        /// </summary>
        private HttpClient Instance { 
            get 
            {
                if (_instance == null)
                {
                    if (this.MessageHandler != null)
                        _instance = new HttpClient(this.MessageHandler);
                    else
                        _instance = new HttpClient();
                }
                return _instance;
            }
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restrequest)
        {
            return await ExecuteAsync<T>(restrequest, new CancellationToken());
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restrequest, CancellationToken cancellationToken)
        {
            var restresponse = await ExecuteAsync(restrequest, cancellationToken);
            var data = Deserialize<T>(restrequest, restresponse);
            return data;
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest restrequest, CancellationToken cancellationToken)
        {
            this.Instance.Timeout = new TimeSpan(0, 0, this.Timeout);

            if (!string.IsNullOrWhiteSpace(this.UserAgent))
            {
                this.Instance.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);
            }

            this.Instance.DefaultRequestHeaders.Add("Accept", "application/json");
            this.Instance.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");

            var handler = new HttpClientHandler();
            if (this.Proxy != null) { handler.Proxy = this.Proxy; }

            var request = ConfigureRequestMessage(restrequest);

            var restresponse = new RestResponse() { ResponseStatus = ResponseStatus.None };

            try
            {
                var response = await this.Instance.SendAsync(request, cancellationToken);

                restresponse.StatusCode = response.StatusCode;
                restresponse.StatusDescription = response.ReasonPhrase;

                if (response.Content!=null)
                    restresponse.RawBytes = await response.Content.ReadAsByteArrayAsync();

                restresponse.ResponseStatus = ResponseStatus.Completed;
            }
            catch (TaskCanceledException exc)
            {
                restresponse.ErrorException = exc;
                restresponse.ErrorMessage = exc.Message;
                restresponse.ResponseStatus = ResponseStatus.Error;
            }
            catch (Exception exc)
            {
                restresponse.ErrorException = exc;
                restresponse.ErrorMessage = exc.Message;
                restresponse.ResponseStatus = ResponseStatus.Error;

                Debug.WriteLine(exc.Message);
            }

            return restresponse;
        }

        public HttpRequestMessage ConfigureRequestMessage(RestRequest restrequest)
        {
            foreach (var param in this.DefaultParameters)
            {
                if (!restrequest.Parameters.Any(p => p.Name == param.Name))
                {
                    restrequest.Parameters.Add(param);
                }
            }

            var request = new HttpRequestMessage(new HttpMethod(restrequest.Method), Simple.UriBuilder.Build(this.BaseUrl, restrequest));

            foreach (var param in restrequest.Parameters.Where(p => p.Type == ParameterType.HttpHeader))
            {
                request.Headers.Add(param.Name, param.Value.ToString());
            }

            var @params = restrequest.Parameters
                                .Where(p => p.Type == ParameterType.GetOrPost && p.Value != null)
                                .Select(p => new KeyValuePair<string, string>(p.Name, p.Value.ToString()));

            if (restrequest.Method == "POST" || restrequest.Method == "PUT")
            {
                request.Content = new FormUrlEncodedContent(@params);
            }

            return request;
        }
    }
}
