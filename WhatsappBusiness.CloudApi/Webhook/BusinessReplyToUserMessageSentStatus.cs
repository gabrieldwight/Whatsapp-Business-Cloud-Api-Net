using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class BusinessReplyToUserMessageSentStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<BusinessReplyToUserEntry> Entry { get; set; }
    }

    public class BusinessReplyToUserEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<BusinessReplyToUserChange> Changes { get; set; }
    }

    public class BusinessReplyToUserChange
    {
        [JsonProperty("value")]
        public BusinessReplyToUserValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class BusinessReplyToUserValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public BusinessReplyToUserMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<BusinessReplyToUserStatus> Statuses { get; set; }
    }

    public class BusinessReplyToUserMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class BusinessReplyToUserStatus
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
        public BusinessReplyToUserConversation Conversation { get; set; }

        [JsonProperty("pricing")]
        public BusinessReplyToUserPricing Pricing { get; set; }
    }

    public class BusinessReplyToUserConversation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiration_timestamp")]
        public string ExpirationTimestamp { get; set; }

        [JsonProperty("origin")]
        public BusinessReplyToUserOrigin Origin { get; set; }
    }

    public class BusinessReplyToUserOrigin
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class BusinessReplyToUserPricing
    {
        [JsonProperty("billable")]
        public bool Billable { get; set; }

        [JsonProperty("pricing_model")]
        public string PricingModel { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}