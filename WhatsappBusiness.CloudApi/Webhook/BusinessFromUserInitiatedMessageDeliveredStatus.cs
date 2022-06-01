using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation (if that conversation did not originate in a free entry point)
    /// </summary>
    public class BusinessFromUserInitiatedMessageDeliveredStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<BusinessFromUserInitiatedEntry> Entry { get; set; }
    }

    public class BusinessFromUserInitiatedEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<BusinessFromUserInitiatedChange> Changes { get; set; }
    }

    public class BusinessFromUserInitiatedChange
    {
        [JsonProperty("value")]
        public BusinessFromUserInitiatedValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class BusinessFromUserInitiatedValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public BusinessFromUserInitiatedMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<BusinessFromUserInitiatedStatus> Statuses { get; set; }
    }

    public class BusinessFromUserInitiatedMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessFromUserInitiatedStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("conversation")]
        public BusinessFromUserInitiatedConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public BusinessFromUserInitiatedPricing Pricing { get; set; }
    }

    public class BusinessFromUserInitiatedConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class BusinessFromUserInitiatedPricing
    {
        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}