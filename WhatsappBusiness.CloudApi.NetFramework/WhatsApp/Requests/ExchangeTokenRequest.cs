using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Requests
{
    /// <summary>
    /// Request model for exchanging authorization code for access token
    /// </summary>
    public class ExchangeTokenRequest
    {
        /// <summary>
        /// Grant type for OAuth flow
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "authorization_code";

        /// <summary>
        /// Client ID (App ID)
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret (App Secret)
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Authorization code received from OAuth flow
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Redirect URI used in OAuth flow
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RedirectUri { get; set; }
    }
}