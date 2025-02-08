using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A text message you receive from a customer, when you have the show_security_notifications parameter set to true in the application settings.
    /// </summary>
    public class TextMessageSecurityNotificationReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<TextMessageSecurityNotificationEntry> Entry { get; set; }
    }

    public class TextMessageSecurityNotificationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<TextMessageSecurityNotificationChange> Changes { get; set; }
    }

    public class TextMessageSecurityNotificationChange
    {
        [JsonPropertyName("value")]
        public TextMessageSecurityNotificationValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class TextMessageSecurityNotificationValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public TextMessageSecurityNotificationMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<TextMessageSecurityNotificationContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<TextMessageSecurityNotificationMessage> Messages { get; set; }
    }

    public class TextMessageSecurityNotificationContact
    {
        [JsonPropertyName("profile")]
        public TextMessageSecurityNotificationProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class TextMessageSecurityNotificationProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TextMessageSecurityNotificationMessage
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("identity")]
        public Identity Identity { get; set; }

        [JsonPropertyName("text")]
        public TextMessageSecurityNotificationText Text { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Identity
    {
        [JsonPropertyName("acknowledged")]
        public bool Acknowledged { get; set; }

        [JsonPropertyName("created_timestamp")]
        public long CreatedTimestamp { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }

    public class TextMessageSecurityNotificationText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class TextMessageSecurityNotificationMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}