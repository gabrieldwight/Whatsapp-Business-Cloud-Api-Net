using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Webhook
{
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
}