using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business sends a message as part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class UserInitiatedMessageSentStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<UserInitiatedEntry> Entry { get; set; }
    }

    public class UserInitiatedEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<UserInitiatedChange> Changes { get; set; }
    }

    public class UserInitiatedChange
    {
        [JsonPropertyName("value")]
        public UserInitiatedValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class UserInitiatedValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public UserInitiatedMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<UserInitiatedStatus> Statuses { get; set; }
    }

    public class UserInitiatedMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class UserInitiatedStatus
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
        public UserInitiatedConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public UserInitiatedPricing Pricing { get; set; }
    }

    public class UserInitiatedConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public UserInitiatedOrigin Origin { get; set; }
    }

    public class UserInitiatedOrigin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class UserInitiatedPricing
    {
        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}