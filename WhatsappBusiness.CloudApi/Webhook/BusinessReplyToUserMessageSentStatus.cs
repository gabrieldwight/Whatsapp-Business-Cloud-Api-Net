using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class BusinessReplyToUserMessageSentStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<BusinessReplyToUserEntry> Entry { get; set; }
    }

    public class BusinessReplyToUserEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<BusinessReplyToUserChange> Changes { get; set; }
    }

    public class BusinessReplyToUserChange
    {
        [JsonPropertyName("value")]
        public BusinessReplyToUserValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class BusinessReplyToUserValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public BusinessReplyToUserMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<BusinessReplyToUserStatus> Statuses { get; set; }
    }

    public class BusinessReplyToUserMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessReplyToUserStatus
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; }

        [JsonPropertyName("conversation")]
        public BusinessReplyToUserConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public BusinessReplyToUserPricing Pricing { get; set; }
    }

    public class BusinessReplyToUserConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public BusinessReplyToUserOrigin Origin { get; set; }
    }

    public class BusinessReplyToUserOrigin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class BusinessReplyToUserPricing
    {
        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}