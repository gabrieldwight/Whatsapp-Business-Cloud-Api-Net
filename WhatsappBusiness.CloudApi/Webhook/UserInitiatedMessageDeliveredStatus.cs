using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class UserInitiatedMessageDeliveredStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<UserInitiatedMessageDeliveryEntry> Entry { get; set; }
    }

    public class UserInitiatedMessageDeliveryEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<UserInitiatedMessageDeliveryChange> Changes { get; set; }
    }

    public class UserInitiatedMessageDeliveryChange
    {
        [JsonPropertyName("value")]
        public UserInitiatedMessageDeliveryValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class UserInitiatedMessageDeliveryValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public UserInitiatedMessageDeliveryMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<UserInitiatedMessageDeliveryStatus> Statuses { get; set; }
    }

    public class UserInitiatedMessageDeliveryMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class UserInitiatedMessageDeliveryStatus
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
        public UserInitiatedMessageDeliveryConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public UserInitiatedMessageDeliveryPricing Pricing { get; set; }
    }

    public class UserInitiatedMessageDeliveryConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("origin")]
        public UserInitiatedMessageDeliveryOrigin Origin { get; set; }
    }

    public class UserInitiatedMessageDeliveryOrigin
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class UserInitiatedMessageDeliveryPricing
    {
        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("billable")]
        public bool Billable { get; set; }
    }
}