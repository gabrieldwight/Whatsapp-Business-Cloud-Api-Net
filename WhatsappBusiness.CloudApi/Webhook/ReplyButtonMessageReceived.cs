using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    public class ReplyButtonMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<ReplyButtonMessageEntry> Entry { get; set; }
    }

    public class ReplyButtonMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<ReplyButtonMessageChange> Changes { get; set; }
    }

    public class ReplyButtonMessageChange
    {
        [JsonPropertyName("value")]
        public ReplyButtonMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class ReplyButtonMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public ReplyButtonMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<ReplyButtonMessageContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<ReplyButtonMessage> Messages { get; set; }
    }

    public class ReplyButtonMessageContact
    {
        [JsonPropertyName("profile")]
        public ReplyButtonMessageProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class ReplyButtonMessageProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ReplyButtonMessage
    {
        [JsonPropertyName("context")]
        public ReplyButtonMessageContext Context { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("interactive")]
        public ReplyButtonMessageInteractive Interactive { get; set; }
    }

    public class ReplyButtonMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class ReplyButtonMessageInteractive
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("button_reply")]
        public ButtonReply ButtonReply { get; set; }
    }

    public class ButtonReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class ReplyButtonMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}