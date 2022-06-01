using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    public class ReplyButtonMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ReplyButtonMessageEntry> Entry { get; set; }
    }

    public class ReplyButtonMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ReplyButtonMessageChange> Changes { get; set; }
    }

    public class ReplyButtonMessageChange
    {
        [JsonProperty("value")]
        public ReplyButtonMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ReplyButtonMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ReplyButtonMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ReplyButtonMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ReplyButtonMessage> Messages { get; set; }
    }

    public class ReplyButtonMessageContact
    {
        [JsonProperty("profile")]
        public ReplyButtonMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ReplyButtonMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ReplyButtonMessage
    {
        [JsonProperty("context")]
        public ReplyButtonMessageContext Context { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("interactive")]
        public ReplyButtonMessageInteractive Interactive { get; set; }
    }

    public class ReplyButtonMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ReplyButtonMessageInteractive
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("button_reply")]
        public ButtonReply ButtonReply { get; set; }
    }

    public class ButtonReply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class ReplyButtonMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}