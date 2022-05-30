using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A Webhook with failed status
    /// </summary>
    public class UserFailedMessageStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<UserFailedMessageEntry> Entry { get; set; }
    }

    public class UserFailedMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<UserFailedMessageChange> Changes { get; set; }
    }

    public class UserFailedMessageChange
    {
        [JsonProperty("value")]
        public UserFailedMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class UserFailedMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public UserFailedMessageMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<UserFailedStatus> Statuses { get; set; }
    }

    public class UserFailedMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class UserFailedStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("errors")]
        public List<UserFailedMessageError> Errors { get; set; }
    }

    public class UserFailedMessageError
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}