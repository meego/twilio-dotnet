﻿using System;
using Simple;
using System.Threading.Tasks;

namespace Twilio
{
    public partial class TwilioRestClient
    {
        /// <summary>
        /// Retrieve the details for a specific SMS message instance.
        /// Makes a GET request to an SMSMessage Instance resource.
        /// </summary>
        /// <param name="smsMessageSid">The Sid of the message to retrieve</param>
        public virtual async Task<SMSMessage> GetSmsMessageAsync(string smsMessageSid)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/SMS/Messages/{SMSMessageSid}.json";
            request.AddUrlSegment("SMSMessageSid", smsMessageSid);

            return await Execute<SMSMessage>(request);
        }

        /// <summary>
        /// Returns a list of SMS messages. 
        /// The list includes paging information.
        /// Makes a GET request to the SMSMessage List resource.
        /// </summary>
        public virtual async Task<SmsMessageResult> ListSmsMessagesAsync()
        {
            return await ListSmsMessagesAsync(null, null, null, null, null);
        }

        /// <summary>
        /// Returns a filtered list of SMS messages. The list includes paging information.
        /// Makes a GET request to the SMSMessages List resource.
        /// </summary>
        /// <param name="to">(Optional) The phone number of the message recipient</param>
        /// <param name="from">(Optional) The phone number of the message sender</param>
        /// <param name="dateSent">(Optional) The date the message was sent (GMT)</param>
        /// <param name="pageNumber">(Optional) The page to start retrieving results from</param>
        /// <param name="count">(Optional) The number of results to retrieve</param>
        public virtual async Task<SmsMessageResult> ListSmsMessagesAsync(string to, string from, DateTime? dateSent, int? pageNumber, int? count)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/SMS/Messages.json";

            if (to.HasValue()) request.AddParameter("To", to);
            if (from.HasValue()) request.AddParameter("From", from);
            if (dateSent.HasValue) request.AddParameter("DateSent", dateSent.Value.ToString("yyyy-MM-dd"));
            if (pageNumber.HasValue) request.AddParameter("Page", pageNumber.Value);
            if (count.HasValue) request.AddParameter("PageSize", count.Value);

            return await Execute<SmsMessageResult>(request);
        }

        /// <summary>
        /// Send a new SMS message to the specified recipients.
        /// Makes a POST request to the SMSMessages List resource.
        /// </summary>
        /// <param name="from">The phone number to send the message from. Must be a Twilio-provided or ported local (not toll-free) number. Validated outgoing caller IDs cannot be used.</param>
        /// <param name="to">The phone number to send the message to. If using the Sandbox, this number must be a validated outgoing caller ID</param>
        /// <param name="body">The message to send. Must be 160 characters or less.</param>
        public virtual async Task<SMSMessage> SendSmsMessageAsync(string from, string to, string body)
        {
            return await SendSmsMessageAsync(from, to, body, string.Empty);
        }

        /// <summary>
        /// Send a new SMS message to the specified recipients
        /// Makes a POST request to the SMSMessages List resource.
        /// </summary>
        /// <param name="from">The phone number to send the message from. Must be a Twilio-provided or ported local (not toll-free) number. Validated outgoing caller IDs cannot be used.</param>
        /// <param name="to">The phone number to send the message to. If using the Sandbox, this number must be a validated outgoing caller ID</param>
        /// <param name="body">The message to send. Must be 160 characters or less.</param>
        /// <param name="statusCallback">A URL that Twilio will POST to when your message is processed. Twilio will POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed</param>
        public virtual async Task<SMSMessage> SendSmsMessageAsync(string from, string to, string body, string statusCallback)
        {
            return await SendSmsMessageAsync(from, to, body, statusCallback, string.Empty);
        }

        /// <summary>
        /// Send a new SMS message to the specified recipients
        /// Makes a POST request to the SMSMessages List resource.
        /// </summary>
        /// <param name="from">The phone number to send the message from. Must be a Twilio-provided or ported local (not toll-free) number. Validated outgoing caller IDs cannot be used.</param>
        /// <param name="to">The phone number to send the message to. If using the Sandbox, this number must be a validated outgoing caller ID</param>
        /// <param name="body">The message to send. Must be 160 characters or less.</param>
        /// <param name="statusCallback">A URL that Twilio will POST to when your message is processed. Twilio will POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed</param>
        /// <param name="applicationSid">Twilio will POST SmsSid as well as SmsStatus=sent or SmsStatus=failed to the URL in the SmsStatusCallback property of this Application. If the StatusCallback parameter above is also passed, the Application's SmsStatusCallback parameter will take precedence.</param>
        public virtual async Task<SMSMessage> SendSmsMessageAsync(string from, string to, string body, string statusCallback, string applicationSid)
        {
            Require.Argument("from", from);
            Require.Argument("to", to);
            Require.Argument("body", body);

            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/SMS/Messages.json";
            request.AddParameter("From", from);
            request.AddParameter("To", to);
            request.AddParameter("Body", body);
            if (statusCallback.HasValue()) request.AddParameter("StatusCallback", statusCallback);
            if (applicationSid.HasValue()) request.AddParameter("ApplicationSid", statusCallback);

            return await Execute<SMSMessage>(request);
        }
        
        /// <summary>
        /// Retrieve the details for a specific ShortCode instance.
        /// Makes a GET request to a ShortCode Instance resource.
        /// </summary>
        /// <param name="shortCodeSid">The Sid of the ShortCode resource to return</param>
        public virtual async Task<SMSShortCode> GetShortCodeAsync(string shortCodeSid)
        {
            Require.Argument("shortCodeSid", shortCodeSid);

            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/SMS/ShortCodes/{ShortCodeSid}.json";
            request.AddUrlSegment("ShortCodeSid", shortCodeSid);

            return await Execute<SMSShortCode>(request);
        }

        /// <summary>
        /// Tries to update the shortcode's properties, and returns the updated resource representation if successful.
        /// Makes a POST request to the ShortCode instance resource.
        /// </summary>
        /// <param name="shortCodeSid">The Sid of the ShortCode instance to update</param>
        /// <param name="friendlyName">A human readable description of the short code, with maximum length 64 characters.</param>
        /// <param name="apiVersion">SMSs to this short code will start a new TwiML session with this API version.</param>
        /// <param name="smsUrl">The URL that Twilio should request when somebody sends an SMS to the short code.</param>
        /// <param name="smsMethod">The HTTP method that should be used to request the SmsUrl. Either GET or POST.</param>
        /// <param name="smsFallbackUrl">A URL that Twilio will request if an error occurs requesting or executing the TwiML at the SmsUrl.</param>
        /// <param name="smsFallbackMethod">The HTTP method that should be used to request the SmsFallbackUrl. Either GET or POST.</param>
        public virtual async Task<SMSShortCode> UpdateShortCodeAsync(string shortCodeSid, string friendlyName, string apiVersion, string smsUrl, string smsMethod, string smsFallbackUrl, string smsFallbackMethod)
        {
            Require.Argument("shortCodeSid", shortCodeSid);

            var request = new RestRequest(Method.POST);
            request.Resource = "Accounts/{AccountSid}/SMS/ShortCodes/{ShortCodeSid}.json";
            request.AddUrlSegment("ShortCodeSid", shortCodeSid);

            if (friendlyName.HasValue()) request.AddParameter("FriendlyName", friendlyName);
            if (apiVersion.HasValue()) request.AddParameter("ApiVersion", apiVersion);
            if (smsUrl.HasValue()) request.AddParameter("SmsUrl", smsUrl);
            if (smsMethod.HasValue()) request.AddParameter("SmsMethod", smsMethod);
            if (smsFallbackUrl.HasValue()) request.AddParameter("SmsFallbackUrl", smsFallbackUrl);
            if (smsFallbackMethod.HasValue()) request.AddParameter("SmsFallbackMethod", smsFallbackMethod);

            return await Execute<SMSShortCode>(request);
        }

        /// <summary>
        /// Returns a list of ShortCode resource representations, each representing a short code within your account.
        /// </summary>
        /// <param name="shortCode">Only show the ShortCode resources that match this pattern. You can specify partial numbers and use '*' as a wildcard for any digit.</param>
        /// <param name="friendlyName">Only show the ShortCode resources with friendly names that exactly match this name.</param>
        public virtual async Task<SmsShortCodeResult> ListShortCodesAsync(string shortCode, string friendlyName)
        {
            var request = new RestRequest();
            request.Resource = "Accounts/{AccountSid}/SMS/ShortCodes.json";

            if (shortCode.HasValue()) request.AddParameter("ShortCode", shortCode);
            if (friendlyName.HasValue()) request.AddParameter("FriendlyName", friendlyName);

            return await Execute<SmsShortCodeResult>(request);
        }
    }
}
