using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    /// <summary>
    /// Response for getting detailed WhatsApp Business Account information
    /// </summary>
    public class WABADetailsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("timezone_id")]
        public string? TimezoneId { get; set; }

        [JsonPropertyName("message_template_namespace")]
        public string? MessageTemplateNamespace { get; set; }

        [JsonPropertyName("account_review_status")]
        public string? AccountReviewStatus { get; set; }

        [JsonPropertyName("business_verification_status")]
        public string? BusinessVerificationStatus { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("owner_business_info")]
        public OwnerBusinessInfo? OwnerBusinessInfo { get; set; }

        [JsonPropertyName("primary_business_location")]
        public string? PrimaryBusinessLocation { get; set; }

        [JsonPropertyName("purchase_order_number")]
        public string? PurchaseOrderNumber { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("health_status")]
        public HealthStatus? HealthStatus { get; set; }
    }
}
