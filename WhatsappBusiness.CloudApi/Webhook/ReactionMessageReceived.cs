using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A reaction message you received from a customer
    /// </summary>
    public class ReactionMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ReactionMessageEntry> Entry { get; set; }
    }

    public class ReactionMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ReactionMessageChange> Changes { get; set; }
    }

    public class ReactionMessageChange
    {
        [JsonProperty("value")]
        public ReactionMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ReactionMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ReactionMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ReactionMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ReactionMessage> Messages { get; set; }
    }

    public class ReactionMessageContact
    {
        [JsonProperty("profile")]
        public ReactionMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ReactionMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ReactionMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reaction")]
        public ReactionMessageText Reaction { get; set; }
    }

    public class ReactionMessageText
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("emoji")]
        public string Emoji { get; set; }
    }

    public class ReactionMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}