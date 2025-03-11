using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class ListReplyButtonMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<ListReplyButtonMessageEntry> Entry { get; set; }
    }

    public class ListReplyButtonMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<ListReplyButtonMessageChange> Changes { get; set; }
    }

    public class ListReplyButtonMessageChange
    {
        [JsonPropertyName("value")]
        public ListReplyButtonMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class ListReplyButtonMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public ListReplyButtonMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<ListReplyButtonMessageContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<ListReplyButtonMessage> Messages { get; set; }
    }

    public class ListReplyButtonMessageContact
    {
        [JsonPropertyName("profile")]
        public ListReplyButtonMessageProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class ListReplyButtonMessageProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ListReplyButtonMessage : IMessage
    {
        [JsonPropertyName("context")]
        public ListReplyButtonMessageContext Context { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("interactive")]
        public Interactive Interactive { get; set; }
    }

    public class ListReplyButtonMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class Interactive
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("list_reply")]
        public ListReply ListReply { get; set; }
    }

    public partial class ListReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class ListReplyButtonMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}