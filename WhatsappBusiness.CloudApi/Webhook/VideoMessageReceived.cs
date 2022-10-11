using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class VideoMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<VideoMessageEntry> Entry { get; set; }
    }

    public class VideoMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<VideoMessageChange> Changes { get; set; }
    }

    public class VideoMessageChange
    {
        [JsonProperty("value")]
        public VideoMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class VideoMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public VideoMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<VideoMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<VideoMessage> Messages { get; set; }
    }

    public class VideoMessageContact
    {
        [JsonProperty("profile")]
        public VideoMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class VideoMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class VideoMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("context")]
        public VideoMessageContext? Context { get; set; }
    }

    public class Video
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class VideoMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class VideoMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}

