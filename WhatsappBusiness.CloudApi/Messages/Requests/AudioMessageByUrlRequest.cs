using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class AudioMessageByUrlRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "audio";

        [JsonProperty("audio")]
        public MediaAudioUrl Audio { get; set; }
    }

    public class MediaAudioUrl
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
