using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A Webhook with failed status
    /// </summary>
    public class UserFailedMessageStatus
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<UserFailedMessageEntry> Entry { get; set; }
    }

    public class UserFailedMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<UserFailedMessageChange> Changes { get; set; }
    }

    public class UserFailedMessageChange
    {
        [JsonPropertyName("value")]
        public UserFailedMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class UserFailedMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public MessageMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<UserFailedStatus> Statuses { get; set; }
    }
    

    public class UserFailedStatus
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; }

        [JsonPropertyName("errors")]
        public List<UserFailedMessageError> Errors { get; set; }
    }

    public class UserFailedMessageError
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}