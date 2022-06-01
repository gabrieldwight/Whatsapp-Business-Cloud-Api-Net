using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// The WhatsApp Business API sends notifications to inform you of the status of the messages between you and users. When a message is sent successfully, you receive a notification when the message is sent, delivered, and read. The order of these notifications in your app may not reflect the actual timing of the message status. You can view the timestamp to determine the timing.
    /// </summary>
    public class MessageStatusUpdateNotification
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<MessageStatusUpdateEntry> Entry { get; set; }
    }

    public class MessageStatusUpdateEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<MessageStatusUpdateChange> Changes { get; set; }
    }

    public class MessageStatusUpdateChange
    {
        [JsonProperty("value")]
        public MessageStatusUpdateValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class MessageStatusUpdateValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public MessageStatusUpdateMetadata Metadata { get; set; }

        [JsonProperty("statuses")]
        public List<MessageStatus> Statuses { get; set; }
    }

    public class MessageStatusUpdateMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class MessageStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }
    }
}