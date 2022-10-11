using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When you receive a media message containing a sticker, WhatsApp Business API downloads the sticker and a notification is sent to your Webhook once the sticker is downloaded.
    /// The Webhook notification contains information that identifies the media object and allows you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class StickerMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<StickerMessageEntry> Entry { get; set; }
    }

    public class StickerMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<StickerMessageChange> Changes { get; set; }
    }

    public class StickerMessageChange
    {
        [JsonProperty("value")]
        public StickerMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class StickerMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public StickerMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<StickerMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<StickerMessage> Messages { get; set; }
    }

    public class StickerMessageContact
    {
        [JsonProperty("profile")]
        public StickerMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class StickerMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class StickerMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sticker")]
        public Sticker Sticker { get; set; }

        [JsonProperty("context")]
        public StickerMessageContext? Context { get; set; }
    }

    public class Sticker
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }
    }

    public class StickerMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class StickerMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}