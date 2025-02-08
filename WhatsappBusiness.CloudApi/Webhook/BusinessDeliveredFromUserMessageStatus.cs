using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation originating from a free entry point
    /// </summary>
    public class BusinessDeliveredFromUserMessageStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<BusinessDeliveredFromUserEntry> Entry { get; set; }
    }

    public class BusinessDeliveredFromUserEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<BusinessDeliveredFromUserChange> Changes { get; set; }
    }

    public class BusinessDeliveredFromUserChange
    {
        [JsonPropertyName("value")]
        public BusinessDeliveredFromUserValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class BusinessDeliveredFromUserValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public BusinessDeliveredFromUserMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<BusinessDeliveredFromUserStatus> Statuses { get; set; }
    }

    public class BusinessDeliveredFromUserMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessDeliveredFromUserStatus
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
        public BusinessDeliveredFromUserConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public BusinessDeliveredFromUserPricing Pricing { get; set; }
    }

    public class BusinessDeliveredFromUserConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public BusinessDeliveredFromUserOrigin Origin { get; set; }
    }

    public class BusinessDeliveredFromUserOrigin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class BusinessDeliveredFromUserPricing
    {
        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}