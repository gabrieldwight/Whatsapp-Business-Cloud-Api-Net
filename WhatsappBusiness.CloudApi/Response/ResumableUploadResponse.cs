using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ResumableUploadResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("file_offset")]
        public long FileOffset { get; set; }

        [JsonPropertyName("h")]
        public string H { get; set; }
    }
}
