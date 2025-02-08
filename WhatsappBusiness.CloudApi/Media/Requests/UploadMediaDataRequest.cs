using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Media.Requests
{
    public class UploadMediaDataRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";
        
        /// <summary>
        /// Name of the file. For example: "file.jpg".
        /// </summary>
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Type of media file being uploaded.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Full file content data.
        /// </summary>
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
    }
}