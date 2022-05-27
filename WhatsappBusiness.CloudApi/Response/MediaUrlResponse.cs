using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class MediaUrlResponse
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("file_size")]
        public string FileSize { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
