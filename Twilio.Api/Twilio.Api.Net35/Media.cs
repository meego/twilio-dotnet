using Simple;
using System;

namespace Twilio
{
    public partial class TwilioRestClient
    {

        /// <summary>
        /// Get the details for a specific Media instance.
        /// </summary>
        /// <param name="messageSid">The Sid of the message this media resource is associated with</param>
        /// <param name="mediaSid">The Sid of the media resource</param>
        /// <returns></returns>
        public virtual Media GetMessageMedia(string messageSid, string mediaSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media/{MediaSid}.json";
            request.AddUrlSegment("MediaSid", mediaSid);
            request.AddUrlSegment("MessageSid", messageSid);
            return Execute<Media>(request);
        }

        /// <summary>
        /// Retrieve a list of Media objects with no list filters
        /// </summary>
        /// <param name="messageSid">The Sid of the message to get media resources for</param>
        [Obsolete("Use ListMessageMedia method instead.")]
        public virtual MediaResult ListMedia(string messageSid)
        {
            return ListMedia(messageSid, new MediaListRequest());
        }

        /// <summary>
        /// Return a filtered list of Media objects. The list includes paging
        /// information.
        /// </summary>
        /// <param name="messageSid">The Sid of the message to get media resources for</param>
        /// <param name="options"></param>
        [Obsolete("Use ListMessageMedia methods instead.")]
        public virtual MediaResult ListMedia(string messageSid, MediaListRequest options)
        {
            return ListMessageMedia(messageSid, options);
        }

        /// <summary>
        /// List all media for a particular message
        /// </summary>
        /// <param name="messageSid">The Sid of the message to get media resources for</param>
        public virtual MediaResult ListMessageMedia(string messageSid)
        {
            return ListMessageMedia(messageSid, new MediaListRequest());
        }

        /// <summary>
        /// List all media for a particular message
        /// </summary>
        /// <param name="messageSid">The message sid to filter on</param>
        /// <param name="options"></param>
        public virtual MediaResult ListMessageMedia(string messageSid, MediaListRequest options)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media.json";
            request.AddUrlSegment("MessageSid", messageSid);
            AddMediaListOptions(options, request);
            return Execute<MediaResult>(request);
        }

        /// <summary>
        /// Delete the specified media instance. Makes a DELETE request to a 
        /// Media Instance resource.
        /// </summary>
        /// <param name="messageSid"></param>
        /// <param name="mediaSid">The Sid of the media to delete</param>
        public virtual DeleteStatus DeleteMessageMedia(string messageSid, string mediaSid)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Messages/{MessageSid}/Media/{MediaSid}.json";

            request.AddParameter("MediaSid", mediaSid, ParameterType.UrlSegment);
            request.AddParameter("MessageSid", messageSid, ParameterType.UrlSegment);

            var response = Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }
    }
}
