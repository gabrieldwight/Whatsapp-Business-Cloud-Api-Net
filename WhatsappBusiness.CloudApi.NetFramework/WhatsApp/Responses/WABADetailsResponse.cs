using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Responses
{
    /// <summary>
    /// Response model for WhatsApp Business Account details
    /// </summary>
    public class WABADetailsResponse
    {
        /// <summary>
        /// WABA ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// WABA name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Timezone ID
        /// </summary>
        [JsonPropertyName("timezone_id")]
        public string TimezoneId { get; set; }

        /// <summary>
        /// Message template namespace
        /// </summary>
        [JsonPropertyName("message_template_namespace")]
        public string MessageTemplateNamespace { get; set; }

        /// <summary>
        /// Account review status
        /// </summary>
        [JsonPropertyName("account_review_status")]
        public string AccountReviewStatus { get; set; }

        /// <summary>
        /// Business verification status
        /// </summary>
        [JsonPropertyName("business_verification_status")]
        public string BusinessVerificationStatus { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// Owner business information
        /// </summary>
        [JsonPropertyName("owner_business_info")]
        public OwnerBusinessInfo OwnerBusinessInfo { get; set; }

        /// <summary>
        /// Primary business location
        /// </summary>
        [JsonPropertyName("primary_business_location")]
        public string PrimaryBusinessLocation { get; set; }

        /// <summary>
        /// Purchase order number
        /// </summary>
        [JsonPropertyName("purchase_order_number")]
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// Account status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Health status information
        /// </summary>
        [JsonPropertyName("health_status")]
        public HealthStatus HealthStatus { get; set; }
    }

    /// <summary>
    /// Owner business information
    /// </summary>
    public class OwnerBusinessInfo
    {
        /// <summary>
        /// Business name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Business ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Health status information
    /// </summary>
    public class HealthStatus
    {
        [JsonPropertyName("can_send_message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CanSendMessage { get; set; }

        [JsonPropertyName("entities")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HealthEntityInfo> Entities { get; set; }
    }

    public class HealthEntityInfo
    {
        [JsonPropertyName("entity_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EntityType { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        [JsonPropertyName("can_send_message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CanSendMessage { get; set; }

        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HealthError> Errors { get; set; }
    }

    public class HealthError
    {
        [JsonPropertyName("error_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ErrorCode { get; set; }

        [JsonPropertyName("error_description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("possible_solution")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PossibleSolution { get; set; }
    }

    public class WABAAnalytics
    {
        [JsonPropertyName("analytics")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AnalyticsData AnalyticsData { get; set; }
    }

    public class AnalyticsData
    {
        [JsonPropertyName("phone_numbers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PhoneNumberAnalytics> PhoneNumbers { get; set; }

        [JsonPropertyName("country_codes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<CountryCodeAnalytics> CountryCodes { get; set; }
    }

    public class PhoneNumberAnalytics
    {
        [JsonPropertyName("phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("sent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Sent { get; set; }

        [JsonPropertyName("delivered")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Delivered { get; set; }
    }

    public class CountryCodeAnalytics
    {
        [JsonPropertyName("country_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CountryCode { get; set; }

        [JsonPropertyName("sent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Sent { get; set; }

        [JsonPropertyName("delivered")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Delivered { get; set; }
    }
}