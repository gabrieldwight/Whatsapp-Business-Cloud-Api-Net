using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A text message you receive from a customer, when you have the show_security_notifications parameter set to true in the application settings.
    /// </summary>
    public class TextMessageSecurityNotificationReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<TextMessageSecurityNotificationEntry> Entry { get; set; }
    }

    public class TextMessageSecurityNotificationEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<TextMessageSecurityNotificationChange> Changes { get; set; }
    }

    public class TextMessageSecurityNotificationChange
    {
        [JsonProperty("value")]
        public TextMessageSecurityNotificationValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class TextMessageSecurityNotificationValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public TextMessageSecurityNotificationMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<TextMessageSecurityNotificationContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<TextMessageSecurityNotificationMessage> Messages { get; set; }
    }

    public class TextMessageSecurityNotificationContact
    {
        [JsonProperty("profile")]
        public TextMessageSecurityNotificationProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class TextMessageSecurityNotificationProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TextMessageSecurityNotificationMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("identity")]
        public Identity Identity { get; set; }

        [JsonProperty("text")]
        public TextMessageSecurityNotificationText Text { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Identity
    {
        [JsonProperty("acknowledged")]
        public bool Acknowledged { get; set; }

        [JsonProperty("created_timestamp")]
        public long CreatedTimestamp { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public class TextMessageSecurityNotificationText
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class TextMessageSecurityNotificationMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}