using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class AudioMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<AudioMessageEntry> Entry { get; set; }
    }

    public class AudioMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<AudioMessageChange> Changes { get; set; }
    }

    public class AudioMessageChange
    {
        [JsonProperty("value")]
        public AudioMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class AudioMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public AudioMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<AudioMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<AudioMessage> Messages { get; set; }
    }

    public class AudioMessageContact
    {
        [JsonProperty("profile")]
        public AudioMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class AudioMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AudioMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("audio")]
        public Audio Audio { get; set; }

        [JsonProperty("context")]
        public AudioMessageContext? Context { get; set; }
    }

    public class Audio
    {
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class AudioMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class AudioMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
