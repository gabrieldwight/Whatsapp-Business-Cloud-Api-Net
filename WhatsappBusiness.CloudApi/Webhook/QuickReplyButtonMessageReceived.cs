using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    public class QuickReplyButtonMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<QuickReplyButtonMessageEntry> Entry { get; set; }
    }

    public class QuickReplyButtonMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<QuickReplyButtonMessageChange> Changes { get; set; }
    }

    public class QuickReplyButtonMessageChange
    {
        [JsonProperty("value")]
        public QuickReplyButtonMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class QuickReplyButtonMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public QuickReplyButtonMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<QuickReplyButtonMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<QuickReplyButtonMessage> Messages { get; set; }
    }

    public class QuickReplyButtonMessageContact
    {
        [JsonProperty("profile")]
        public QuickReplyButtonMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class QuickReplyButtonMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class QuickReplyButtonMessage
    {
        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("button")]
        public Button Button { get; set; }
    }

    public class Button
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }

    public class Context
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class QuickReplyButtonMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}