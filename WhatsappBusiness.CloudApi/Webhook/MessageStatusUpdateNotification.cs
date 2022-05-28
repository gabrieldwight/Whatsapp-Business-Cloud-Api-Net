using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Webhook
{
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
        public List<Status> Statuses { get; set; }
    }

    public class MessageStatusUpdateMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class Status
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string StatusStatus { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }
    }
}