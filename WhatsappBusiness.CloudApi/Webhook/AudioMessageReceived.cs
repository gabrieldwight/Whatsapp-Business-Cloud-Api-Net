using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class AudioMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<AudioMessageEntry> Entry { get; set; }
    }

    public class AudioMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<AudioMessageChange> Changes { get; set; }
    }

    public class AudioMessageChange
    {
        [JsonPropertyName("value")]
        public AudioMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class AudioMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public AudioMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<AudioMessageContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<AudioMessage> Messages { get; set; }
    }

    public class AudioMessageContact
    {
        [JsonPropertyName("profile")]
        public AudioMessageProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class AudioMessageProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class AudioMessage
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("audio")]
        public Audio Audio { get; set; }

        [JsonPropertyName("context")]
        public AudioMessageContext? Context { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class AudioMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class AudioMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
