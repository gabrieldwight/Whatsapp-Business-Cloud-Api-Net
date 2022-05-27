using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class StickerMessageByUrlRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "sticker";

        [JsonProperty("sticker")]
        public MediaStickerUrl Sticker { get; set; }
    }

    public class MediaStickerUrl
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
