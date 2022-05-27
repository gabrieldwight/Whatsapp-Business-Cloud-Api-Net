using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Media.Requests
{
    public class UploadMediaRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
