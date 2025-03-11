using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// Currently, the Cloud API does not support webhook status updates for deleted messages. If a user deletes a message, you will receive a webhook with an error code for an unsupported message type
    /// </summary>
    public class UserDeletedMessageStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<UserDeletedMessageEntry> Entry { get; set; }
    }

    public class UserDeletedMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<UserDeletedMessageChange> Changes { get; set; }
    }

    public class UserDeletedMessageChange
    {
        [JsonPropertyName("value")]
        public UserDeletedMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class UserDeletedMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public UserDeletedMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<UserDeletedMessage> Messages { get; set; }
    }


    public class UserDeletedMessage : GenericMessage
    {

        [JsonPropertyName("errors")]
        public List<UserDeletedMessageError> Errors { get; set; }

    }

    public class UserDeletedMessageError
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class UserDeletedMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}