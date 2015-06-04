using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Returns a list of Recordings, each representing a recording generated during the course of a phone call. 
        /// The list includes paging information.
        /// Makes a GET request to the Recordings List resource.
        /// </summary>
        public virtual async Task<RecordingResult> ListRecordingsAsync()
        {
            return await ListRecordingsAsync(new { });
        }

        /// <summary>
        /// Returns a list of Recordings, each representing a recording generated during the course of a phone call. 
        /// The list includes paging information.
        /// Makes a GET request to the Recordings List resource.
        /// </summary>
        /// <param name="callsid">Show only recordings made during the call given by this sid</param>
        public virtual async Task<RecordingResult> ListRecordingsAsync(string callSid)
        {
            return await ListRecordingsAsync(new { CallSid = callSid });
        }

        /// <summary>
        /// Returns a filtered list of Recordings, each representing a recording generated during the course of a phone call. 
        /// The list includes paging information.
        /// Makes a GET request to the Recordings List resource.
        /// </summary>
        /// <param name="callSid">Show only recordings made during the call given by this sid</param>
        /// <param name="dateRange">The date the recordings were created (GMT)</param>
        public virtual async Task<RecordingResult> ListRecordingsAsync(string callSid, DateTime dateCreated)
        {
            return await ListRecordingsAsync(new { CallSid = callSid, DateCreated = dateCreated });
        }

        /// <summary>
        /// Returns a filtered list of Recordings, each representing a recording generated during the course of a phone call. 
        /// The list includes paging information.
        /// Makes a GET request to the Recordings List resource.
        /// </summary>
        /// <param name="callSid">Show only recordings made during the call given by this sid</param>
        /// <param name="dateRange">The date range the recordings were created (GMT)</param>
        public virtual async Task<RecordingResult> ListRecordingsAsync(string callSid, DateRange dateRange)
        {
            return await ListRecordingsAsync(new { CallSid = callSid, DateRange = dateRange });
        }


        /// <summary>
        /// Returns a filtered list of Recordings, each representing a recording generated during the course of a phone call. 
        /// The list includes paging information.
        /// Makes a GET request to the Recordings List resource.
        /// <param name="parameters">An object that contains Recording list parameters</param>
        public async Task<RecordingResult> ListRecordingsAsync(object parameters) 
        {
            
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Recordings.json";

            parameters.HasProperty<string>("CallSid", r => { 
                if (r.HasValue()) { request.AddParameter("CallSid", r); } 
            });

            parameters.HasProperty<int>("PageSize", r => request.AddParameter("PageSize", r) );
            parameters.HasProperty<DateTime>("DateCreated", r => request.AddParameter("DateCreated", r) );
            parameters.HasProperty<DateRange>("DateRange", r => request.AddParameter("DateRange", r) );

            return await Execute<RecordingResult>(request);   
        }

        /// <summary>
        /// Retrieve the details for the specified recording instance.
        /// Makes a GET request to a Recording Instance resource.
        /// </summary>
        /// <param name="recordingSid">The Sid of the recording to retrieve</param>
        public virtual async Task<Recording> GetRecordingAsync(string recordingSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.json";
            
            request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

            return await Execute<Recording>(request);
        }

        /// <summary>
        /// Delete the specified recording instance. Makes a DELETE request to a Recording Instance resource.
        /// </summary>
        /// <param name="recordingSid">The Sid of the recording to delete</param>
        public virtual async Task<DeleteStatus> DeleteRecordingAsync(string recordingSid)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.json";
            
            request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Retrieves the transcription text for the specified recording, if it was transcribed. 
        /// Makes a GET request to a Recording Instance resource.
        /// </summary>
        /// <param name="recordingSid">The Sid of the recording to retreive the transcription for</param>
        public virtual async Task<string> GetRecordingTextAsync(string recordingSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.txt";
            request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

            var response = await Execute(request);
            return response.Content;
        }
    }
}
