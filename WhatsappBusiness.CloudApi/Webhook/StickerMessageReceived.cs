using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When you receive a media message containing a sticker, WhatsApp Business API downloads the sticker and a notification is sent to your Webhook once the sticker is downloaded.
    /// The Webhook notification contains information that identifies the media object and allows you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class StickerMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<StickerMessageEntry> Entry { get; set; }
    }

    public class StickerMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<StickerMessageChange> Changes { get; set; }
    }

    public class StickerMessageChange
    {
        [JsonPropertyName("value")]
        public StickerMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class StickerMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public StickerMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<StickerMessage> Messages { get; set; }
    }

    
    public class StickerMessage : GenericMessage
    {        

        [JsonPropertyName("sticker")]
        public Sticker Sticker { get; set; }

        [JsonPropertyName("context")]
        public StickerMessageContext? Context { get; set; }
    }

    public class Sticker
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }
    }

    public class StickerMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class StickerMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}