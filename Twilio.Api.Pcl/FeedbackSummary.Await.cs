using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Creates a feedback summary.
        /// </summary>
        /// <returns>A feedback summary.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public virtual async Task<FeedbackSummary> CreateFeedbackSummary(DateTime startDate, DateTime endDate)
        {
            return await CreateFeedbackSummary(startDate, endDate, false, null, null);
        }

        /// <summary>
        /// Creates a feedback summary.
        /// </summary>
        /// <returns>A feedback summary.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="includeSubaccounts">If set to <c>true</c> include subaccounts.</param>
        public virtual async Task<FeedbackSummary> CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts)
        {
            return await CreateFeedbackSummary(startDate, endDate, includeSubaccounts, null, null);
        }

        /// <summary>
        /// Creates a feedback summary.
        /// </summary>
        /// <returns>A feedback summary.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="includeSubaccounts">If set to <c>true</c> include subaccounts.</param>
        /// <param name="statusCallback">Status callback URL.</param>
        /// <param name="statusCallbackMethod">Status callback URL method. Either GET or POST.</param>
        public virtual async Task<FeedbackSummary> CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts, string statusCallback, string statusCallbackMethod)
        {
            Require.Argument("StartDate", startDate.ToString("yyyy-MM-dd"));
            Require.Argument("EndDate", endDate.ToString("yyyy-MM-dd"));

            var request = new RestRequest();
            request.Method = "POST";
            request.Resource = "Accounts/{AccountSid}/Calls/FeedbackSummary.json";

            request.AddParameter("StartDate", startDate);
            request.AddParameter("EndDate", endDate);
            request.AddParameter("IncludeSubaccounts", includeSubaccounts);
            if (!string.IsNullOrEmpty(statusCallback))
            {
                request.AddParameter("StatusCallback", statusCallback);
            }
            if (!string.IsNullOrEmpty(statusCallbackMethod))
            {
                request.AddParameter("StatusCallbackMethod", statusCallbackMethod);
            }

            return await Execute<FeedbackSummary>(request);
        }

        /// <summary>
        /// Deletes a feedback summary.
        /// </summary>
        /// <returns>Deletion status.</returns>
        /// <param name="feedbackSummarySid">Feedback summary sid.</param>
        public virtual async Task<DeleteStatus> DeleteFeedbackSummary(string feedbackSummarySid)
        {
            Require.Argument("FeedbackSummarySid", feedbackSummarySid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Calls/FeedbackSummary/{Sid}.json";

            request.Method = "DELETE";

            request.AddUrlSegment("Sid", feedbackSummarySid);

            var response = await Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
        }

        /// <summary>
        /// Gets a feedback summary.
        /// </summary>
        /// <returns>A feedback summary.</returns>
        /// <param name="feedbackSummarySid">Feedback summary sid.</param>
        public virtual async Task<FeedbackSummary> GetFeedbackSummary(string feedbackSummarySid)
        {
            Require.Argument("FeedbackSummarySid", feedbackSummarySid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/Calls/FeedbackSummary/{Sid}.json";
            request.AddUrlSegment("Sid", feedbackSummarySid);

            return await Execute<FeedbackSummary>(request);
        }
    }
}