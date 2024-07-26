using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Media.Requests
{
    public class UploadMediaDataRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";
        
        /// <summary>
        /// Name of the file. For example: "file.jpg".
        /// </summary>
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Type of media file being uploaded.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Full file content data.
        /// </summary>
        [JsonProperty("data")]
        public byte[] Data { get; set; }
    }
}