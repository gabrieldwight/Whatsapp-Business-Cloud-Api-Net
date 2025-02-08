using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class BusinessFromUserInitiatedMessageDeliveredStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<BusinessFromUserInitiatedEntry> Entry { get; set; }
    }

    public class BusinessFromUserInitiatedEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<BusinessFromUserInitiatedChange> Changes { get; set; }
    }

    public class BusinessFromUserInitiatedChange
    {
        [JsonPropertyName("value")]
        public BusinessFromUserInitiatedValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class BusinessFromUserInitiatedValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public BusinessFromUserInitiatedMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<BusinessFromUserInitiatedStatus> Statuses { get; set; }
    }

    public class BusinessFromUserInitiatedMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessFromUserInitiatedStatus
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
        public BusinessFromUserInitiatedConversation Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public BusinessFromUserInitiatedPricing Pricing { get; set; }
    }

    public class BusinessFromUserInitiatedConversation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class BusinessFromUserInitiatedPricing
    {
        [JsonPropertyName("billable")]
        public bool Billable { get; set; }

        [JsonPropertyName("pricing_model")]
        public string PricingModel { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}