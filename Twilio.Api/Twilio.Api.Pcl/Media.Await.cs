using Simple;
using System;
using System.Threading.Tasks;

namespace Twilio
{
    public partial class TwilioRestClient
    {

        /// <summary>
        /// Get the details for a specific Media instance.
        /// </summary>
        /// <param name="mediaSid">The Sid of the media resource</param>
        /// <returns></returns>
        public virtual async Task<Media> GetMessageMediaAsync(string messageSid, string mediaSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media/{MediaSid}.json";
            request.AddUrlSegment("MediaSid", mediaSid);
            request.AddUrlSegment("MessageSid", messageSid);
            return await Execute<Media>(request);
        }

        /// <summary>
        /// Retrieve a list of Media objects with no list filters
        /// </summary>
        [Obsolete("Use ListMessageMedia methods instead.")]
        public virtual async Task<MediaResult> ListMediaAsync(string messageSid)
        {
            return await ListMediaAsync(messageSid, new MediaListRequest());
        }

        /// <summary>
        /// Return a filtered list of Media objects. The list includes paging
        /// information.
        /// </summary>
        [Obsolete("Use ListMessageMedia methods instead.")]
        public virtual async Task<MediaResult> ListMediaAsync(string messageSid, MediaListRequest options)
        {
            return await ListMessageMediaAsync(messageSid, options);
        }

        /// <summary>
        /// List all media for a particular message
        /// </summary>
        public virtual async Task<MediaResult> ListMessageMediaAsync(string messageSid)
        {
            return await ListMessageMediaAsync(messageSid, new MediaListRequest());
        }

        /// <summary>
        /// List all media for a particular message
        /// </summary>
        /// <param name="messageSid">The message sid to filter on</param>
        /// <param name="options"></param>
        public virtual async Task<MediaResult> ListMessageMediaAsync(string messageSid, MediaListRequest options)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media.json";
            request.AddUrlSegment("MessageSid", messageSid);
            AddMediaListOptions(options, request);
            return await Execute<MediaResult>(request);
        }

        /// <summary>
        /// Delete the specified media instance. Makes a DELETE request to a 
        /// Media Instance resource.
        /// </summary>
        /// <param name="mediaSid">The Sid of the media to delete</param>
        public virtual async Task<DeleteStatus> DeleteMessageMediaAsync(string messageSid, string mediaSid)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media/{MediaSid}.json";

            request.AddParameter("MediaSid", mediaSid, ParameterType.UrlSegment);
            request.AddParameter("MessageSid", messageSid, ParameterType.UrlSegment);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }
    }
}
