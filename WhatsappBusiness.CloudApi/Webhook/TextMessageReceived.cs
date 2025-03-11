using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A text message you received from a customer
    /// </summary>
    public class TextMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<TextMessageEntry> Entry { get; set; }
    }

    public class TextMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<TextMessageChange> Changes { get; set; }
    }

    public class TextMessageChange
    {
        [JsonPropertyName("value")]
        public TextMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class TextMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public TextMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<TextMessage> Messages { get; set; }
    }

    public class TextMessage : GenericMessage
    {
        [JsonPropertyName("text")]
        public TextMessageText Text { get; set; }

        [JsonPropertyName("context")]
        public TextMessageContext? Context { get; set; }
    }

    public class TextMessageText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class TextMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class TextMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}