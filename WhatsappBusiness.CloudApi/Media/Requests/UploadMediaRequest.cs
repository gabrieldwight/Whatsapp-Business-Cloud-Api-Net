using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Media.Requests
{
    public class UploadMediaRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        /// <summary>
        /// Path to the file stored in your local directory. For example: "@/local/path/file.jpg".
        /// </summary>
        [JsonPropertyName("file")]
        public string File { get; set; }

        /// <summary>
        /// Type of media file being uploaded.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
