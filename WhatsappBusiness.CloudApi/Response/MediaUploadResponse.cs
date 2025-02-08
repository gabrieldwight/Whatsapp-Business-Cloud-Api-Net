using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class MediaUploadResponse
    {
        [JsonPropertyName("id")]
        public string MediaId { get; set; }
    }
}
