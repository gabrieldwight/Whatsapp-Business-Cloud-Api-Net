using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Responses
{
    /// <summary>
    /// Response model for phone number information
    /// </summary>
    public class PhoneNumberResponse
    {
        /// <summary>
        /// List of phone number data
        /// </summary>
        [JsonPropertyName("data")]
        public List<PhoneNumberData> Data { get; set; }

        /// <summary>
        /// Paging information
        /// </summary>
        [JsonPropertyName("paging")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PagingData Paging { get; set; }

        /// <summary>
        /// Gets the phone number ID with the most recent last onboarded time.
        /// </summary>
        /// <returns>The ID of the phone number with the most recent onboarding time, or null if no valid data is found.</returns>
        public string GetMostRecentlyOnboardedPhoneNumberId()
        {
            if (Data == null || !Data.Any())
                return null;

            var phoneNumberWithMostRecentOnboarding = Data
                .Where(pn => !string.IsNullOrEmpty(pn.LastOnboardedTime))
                .Select(pn => new
                {
                    PhoneNumber = pn,
                    ParsedDate = TryParseLastOnboardedTime(pn.LastOnboardedTime)
                })
                .Where(x => x.ParsedDate.HasValue)
                .OrderByDescending(x => x.ParsedDate.Value)
                .FirstOrDefault();

            return phoneNumberWithMostRecentOnboarding?.PhoneNumber.Id;
        }

        /// <summary>
        /// Tries to parse the last onboarded time string in ISO 8601 format.
        /// </summary>
        /// <param name="lastOnboardedTime">The last onboarded time string in format "2025-07-23T10:21:25+0000"</param>
        /// <returns>Parsed DateTime if successful, null otherwise</returns>
        private static DateTime? TryParseLastOnboardedTime(string lastOnboardedTime)
        {
            if (string.IsNullOrEmpty(lastOnboardedTime))
                return null;

            // Try parsing ISO 8601 format with timezone offset
            if (DateTimeOffset.TryParseExact(lastOnboardedTime, "yyyy-MM-ddTHH:mm:sszzz", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeOffset))
            {
                return dateTimeOffset.DateTime;
            }

            // Fallback: try general parsing
            if (DateTimeOffset.TryParse(lastOnboardedTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOffset))
            {
                return dateTimeOffset.DateTime;
            }

            return null;
        }
    }

    /// <summary>
    /// Phone number data
    /// </summary>
    public class PhoneNumberData
    {
        /// <summary>
        /// Verified name
        /// </summary>
        [JsonPropertyName("verified_name")]
        public string VerifiedName { get; set; }

        /// <summary>
        /// Display phone number
        /// </summary>
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        /// <summary>
        /// Phone number ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Quality rating
        /// </summary>
        [JsonPropertyName("quality_rating")]
        public string QualityRating { get; set; }

        /// <summary>
        /// Code verification status
        /// </summary>
        [JsonPropertyName("code_verification_status")]
        public string CodeVerificationStatus { get; set; }

        /// <summary>
        /// Platform type
        /// </summary>
        [JsonPropertyName("platform_type")]
        public string PlatformType { get; set; }

        /// <summary>
        /// Throughput information
        /// </summary>
        [JsonPropertyName("throughput")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ThroughputData Throughput { get; set; }

        /// <summary>
        /// Last onboarded time
        /// </summary>
        [JsonPropertyName("last_onboarded_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LastOnboardedTime { get; set; }

        /// <summary>
        /// Webhook configuration
        /// </summary>
        [JsonPropertyName("webhook_configuration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WebhookConfigurationData WebhookConfiguration { get; set; }
    }

    /// <summary>
    /// Throughput data
    /// </summary>
    public class ThroughputData
    {
        /// <summary>
        /// Throughput level
        /// </summary>
        [JsonPropertyName("level")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Level { get; set; }
    }

    /// <summary>
    /// Webhook configuration data
    /// </summary>
    public class WebhookConfigurationData
    {
        /// <summary>
        /// Application
        /// </summary>
        [JsonPropertyName("application")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Application { get; set; }
    }

    /// <summary>
    /// Paging data
    /// </summary>
    public class PagingData
    {
        /// <summary>
        /// Cursors for paging
        /// </summary>
        [JsonPropertyName("cursors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CursorsData Cursors { get; set; }
    }

    /// <summary>
    /// Cursors data
    /// </summary>
    public class CursorsData
    {
        /// <summary>
        /// Before cursor
        /// </summary>
        [JsonPropertyName("before")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Before { get; set; }

        /// <summary>
        /// After cursor
        /// </summary>
        [JsonPropertyName("after")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string After { get; set; }
    }
}