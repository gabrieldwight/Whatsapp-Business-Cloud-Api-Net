using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The following notification is received when a business’ message is delivered and that message is part of a user-initiated conversation originating from a free entry point
    /// </summary>
    public class BusinessDeliveredFromUserMessageStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<BusinessDeliveredFromUserEntry> Entry { get; set; }
    }

    public class BusinessDeliveredFromUserEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<BusinessDeliveredFromUserChange> Changes { get; set; }
    }

    public class BusinessDeliveredFromUserChange
    {
        [JsonProperty("value")]
        public BusinessDeliveredFromUserValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class BusinessDeliveredFromUserValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public BusinessDeliveredFromUserMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<BusinessDeliveredFromUserStatus> Statuses { get; set; }
    }

    public class BusinessDeliveredFromUserMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessDeliveredFromUserStatus
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
        public BusinessDeliveredFromUserConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public BusinessDeliveredFromUserPricing Pricing { get; set; }
    }

    public class BusinessDeliveredFromUserConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("origin")]
        public BusinessDeliveredFromUserOrigin Origin { get; set; }
    }

    public class BusinessDeliveredFromUserOrigin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class BusinessDeliveredFromUserPricing
    {
        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}