using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class OfficialBusinessAccountStatusResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("oba_status")]
        public string? Status { get; set; }

        [JsonPropertyName("status_message")]
        public string? StatusMessage { get; set; }
    }

    public class OfficialBusinessAccountUpdateResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("updated_status")]
        public OfficialBusinessAccountStatusResponse? UpdatedStatus { get; set; }

        [JsonPropertyName("tracking_id")]
        public string? TrackingId { get; set; }
    }
}
