using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    public class QuickReplyButtonMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<QuickReplyButtonMessageEntry> Entry { get; set; }
    }

    public class QuickReplyButtonMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<QuickReplyButtonMessageChange> Changes { get; set; }
    }

    public class QuickReplyButtonMessageChange
    {
        [JsonPropertyName("value")]
        public QuickReplyButtonMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class QuickReplyButtonMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public QuickReplyButtonMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<QuickReplyButtonMessageContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<QuickReplyButtonMessage> Messages { get; set; }
    }

    public class QuickReplyButtonMessageContact
    {
        [JsonPropertyName("profile")]
        public QuickReplyButtonMessageProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class QuickReplyButtonMessageProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class QuickReplyButtonMessage : IMessage
    {
        [JsonPropertyName("context")]
        public QuickReplyButtonMessageContext Context { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("button")]
        public QuickReplyButtonMessageButton Button { get; set; }
    }

    public class QuickReplyButtonMessageButton
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }

    public class QuickReplyButtonMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class QuickReplyButtonMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}