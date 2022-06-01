using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business sends a message as part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class UserInitiatedMessageSentStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<UserInitiatedEntry> Entry { get; set; }
    }

    public class UserInitiatedEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<UserInitiatedChange> Changes { get; set; }
    }

    public class UserInitiatedChange
    {
        [JsonProperty("value")]
        public UserInitiatedValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class UserInitiatedValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public UserInitiatedMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<UserInitiatedStatus> Statuses { get; set; }
    }

    public class UserInitiatedMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class UserInitiatedStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("conversation")]
        public UserInitiatedConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public UserInitiatedPricing Pricing { get; set; }
    }

    public class UserInitiatedConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("origin")]
        public UserInitiatedOrigin Origin { get; set; }
    }

    public class UserInitiatedOrigin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class UserInitiatedPricing
    {
        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}