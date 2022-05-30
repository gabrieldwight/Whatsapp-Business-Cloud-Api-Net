using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// Currently, the Cloud API does not support webhook status updates for deleted messages. If a user deletes a message, you will receive a webhook with an error code for an unsupported message type
    /// </summary>
    public class UserDeletedMessageStatus
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<UserDeletedMessageEntry> Entry { get; set; }
    }

    public class UserDeletedMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<UserDeletedMessageChange> Changes { get; set; }
    }

    public class UserDeletedMessageChange
    {
        [JsonProperty("value")]
        public UserDeletedMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class UserDeletedMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public UserDeletedMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<UserDeletedMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<UserDeletedMessage> Messages { get; set; }
    }

    public class UserDeletedMessageContact
    {
        [JsonProperty("profile")]
        public UserDeletedMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class UserDeletedMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class UserDeletedMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("errors")]
        public List<UserDeletedMessageError> Errors { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class UserDeletedMessageError
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class UserDeletedMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}