using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class AudioMessageByIdRequest
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
        public MediaAudio Audio { get; set; }
    }

    public class MediaAudio
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
