using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class BaseSuccessResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
