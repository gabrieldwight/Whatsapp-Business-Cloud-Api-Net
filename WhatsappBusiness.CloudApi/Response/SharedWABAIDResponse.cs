using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SharedWABAIDResponse
    {
        [JsonPropertyName("data")]
        public SharedWABAIDData Data { get; set; }
    }

    public class SharedWABAIDData
    {
        [JsonPropertyName("app_id")]
        public string AppId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("application")]
        public string Application { get; set; }

        [JsonPropertyName("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("scopes")]
        public List<string> Scopes { get; set; }

        [JsonPropertyName("granular_scopes")]
        public List<GranularScope> GranularScopes { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }

    public partial class GranularScope
    {
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("target_ids")]
        public List<string> TargetIds { get; set; }
    }
}