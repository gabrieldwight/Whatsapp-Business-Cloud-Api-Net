using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business sends a message as part of a business-initiated conversation
    /// </summary>
    public class BusinessInitiatedMessageSentStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<BusinessInitiatedEntry> Entry { get; set; }
    }

    public class BusinessInitiatedEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<BusinessInitiatedChange> Changes { get; set; }
    }

    public class BusinessInitiatedChange
    {
        [JsonPropertyName("value")]
        public BusinessInitiatedValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class BusinessInitiatedValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public BusinessInitiatedMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<BusinessInitiatedStatus> Statuses { get; set; }
    }

    public class BusinessInitiatedMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessInitiatedStatus
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("conversation")]
        public BusinessInitiatedConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public BusinessInitiatedPricing Pricing { get; set; }
    }

    public class BusinessInitiatedConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public BusinessInitiatedOrigin Origin { get; set; }
    }

    public class BusinessInitiatedOrigin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class BusinessInitiatedPricing
    {
        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}