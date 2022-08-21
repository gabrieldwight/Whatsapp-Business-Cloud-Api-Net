using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Media.Requests
{
    public class UploadMediaRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        /// <summary>
        /// Path to the file stored in your local directory. For example: "@/local/path/file.jpg".
        /// </summary>
        [JsonProperty("file")]
        public string File { get; set; }

        /// <summary>
        /// Type of media file being uploaded.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
