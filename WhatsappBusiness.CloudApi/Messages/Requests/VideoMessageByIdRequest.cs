using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class VideoMessageByIdRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "video";

        [JsonProperty("video")]
        public MediaVideo Video { get; set; }
    }

    public class MediaVideo
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
