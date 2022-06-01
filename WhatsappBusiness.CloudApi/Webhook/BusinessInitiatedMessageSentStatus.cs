using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business sends a message as part of a business-initiated conversation
    /// </summary>
    public class BusinessInitiatedMessageSentStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<BusinessInitiatedEntry> Entry { get; set; }
    }

    public class BusinessInitiatedEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<BusinessInitiatedChange> Changes { get; set; }
    }

    public class BusinessInitiatedChange
    {
        [JsonProperty("value")]
        public BusinessInitiatedValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class BusinessInitiatedValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public BusinessInitiatedMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<BusinessInitiatedStatus> Statuses { get; set; }
    }

    public class BusinessInitiatedMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessInitiatedStatus
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
        public BusinessInitiatedConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public BusinessInitiatedPricing Pricing { get; set; }
    }

    public class BusinessInitiatedConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("origin")]
        public BusinessInitiatedOrigin Origin { get; set; }
    }

    public class BusinessInitiatedOrigin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class BusinessInitiatedPricing
    {
        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}