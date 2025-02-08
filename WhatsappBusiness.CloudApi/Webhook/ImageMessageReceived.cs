using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class ImageMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<ImageMessageEntry> Entry { get; set; }
    }

    public class ImageMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<ImageMessageChange> Changes { get; set; }
    }

    public class ImageMessageChange
    {
        [JsonPropertyName("value")]
        public ImageMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class ImageMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public ImageMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<ImageMessageContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<ImageMessage> Messages { get; set; }
    }

    public class ImageMessageContact
    {
        [JsonPropertyName("profile")]
        public ImageMessageProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class ImageMessageProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ImageMessage
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("context")]
        public ImageMessageContext? Context { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class ImageMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class ImageMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}