using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SharedWABAResponse
    {
        [JsonPropertyName("data")]
        public List<WABAData> Data { get; set; }

        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }
    }

    public class WABAData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("timezone_id")]
        public string TimezoneId { get; set; }

        [JsonPropertyName("message_template_namespace")]
        public string MessageTemplateNamespace { get; set; }

        // Additional properties for detailed WABA information
        [JsonPropertyName("account_review_status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccountReviewStatus { get; set; }

        [JsonPropertyName("business_verification_status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BusinessVerificationStatus { get; set; }

        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Country { get; set; }

        [JsonPropertyName("owner_business_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OwnerBusinessInfo? OwnerBusinessInfo { get; set; }

        [JsonPropertyName("primary_business_location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryBusinessLocation { get; set; }

        [JsonPropertyName("purchase_order_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PurchaseOrderNumber { get; set; }

        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Status { get; set; }

        [JsonPropertyName("health_status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HealthStatus? HealthStatus { get; set; }

        [JsonPropertyName("analytics")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WABAAnalytics? Analytics { get; set; }
    }

    public class OwnerBusinessInfo
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
    }

    public class HealthStatus
    {
        [JsonPropertyName("can_send_message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CanSendMessage { get; set; }

        [JsonPropertyName("entities")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HealthEntityInfo>? Entities { get; set; }
    }

    public class HealthEntityInfo
    {
        [JsonPropertyName("entity_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EntityType { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("can_send_message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CanSendMessage { get; set; }

        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HealthError>? Errors { get; set; }
    }

    public class HealthError
    {
        [JsonPropertyName("error_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ErrorCode { get; set; }

        [JsonPropertyName("error_description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorDescription { get; set; }

        [JsonPropertyName("possible_solution")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PossibleSolution { get; set; }
    }

    public class WABAAnalytics
    {
        [JsonPropertyName("analytics")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AnalyticsData? AnalyticsData { get; set; }
    }

    public class AnalyticsData
    {
        [JsonPropertyName("phone_numbers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PhoneNumberAnalytics>? PhoneNumbers { get; set; }

        [JsonPropertyName("country_codes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<CountryCodeAnalytics>? CountryCodes { get; set; }
    }

    public class PhoneNumberAnalytics
    {
        [JsonPropertyName("phone_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneNumber { get; set; }

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
        public string? CountryCode { get; set; }

        [JsonPropertyName("sent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Sent { get; set; }

        [JsonPropertyName("delivered")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int Delivered { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }
}