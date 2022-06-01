using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class UserInitiatedMessageDeliveredStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<UserInitiatedMessageDeliveryEntry> Entry { get; set; }
    }

    public class UserInitiatedMessageDeliveryEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<UserInitiatedMessageDeliveryChange> Changes { get; set; }
    }

    public class UserInitiatedMessageDeliveryChange
    {
        [JsonProperty("value")]
        public UserInitiatedMessageDeliveryValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class UserInitiatedMessageDeliveryValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public UserInitiatedMessageDeliveryMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<UserInitiatedMessageDeliveryStatus> Statuses { get; set; }
    }

    public class UserInitiatedMessageDeliveryMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class UserInitiatedMessageDeliveryStatus
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
        public UserInitiatedMessageDeliveryConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public UserInitiatedMessageDeliveryPricing Pricing { get; set; }
    }

    public class UserInitiatedMessageDeliveryConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("origin")]
        public UserInitiatedMessageDeliveryOrigin Origin { get; set; }
    }

    public class UserInitiatedMessageDeliveryOrigin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class UserInitiatedMessageDeliveryPricing
    {
        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("billable")]
        public bool Billable { get; set; }
    }
}