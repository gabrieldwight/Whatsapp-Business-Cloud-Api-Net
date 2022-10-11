using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A text message you received from a customer
    /// </summary>
    public class TextMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<TextMessageEntry> Entry { get; set; }
    }

    public class TextMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<TextMessageChange> Changes { get; set; }
    }

    public class TextMessageChange
    {
        [JsonProperty("value")]
        public TextMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class TextMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public TextMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<TextMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<TextMessage> Messages { get; set; }
    }

    public class TextMessageContact
    {
        [JsonProperty("profile")]
        public TextMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class TextMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TextMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("text")]
        public TextMessageText Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("context")]
        public TextMessageContext? Context { get; set; }
    }

    public class TextMessageText
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class TextMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class TextMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}