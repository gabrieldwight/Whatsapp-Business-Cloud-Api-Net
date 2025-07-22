using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.OAuth.Requests
{
    public class ExchangeTokenRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "authorization_code";

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("redirect_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RedirectUri { get; set; }
    }
}
