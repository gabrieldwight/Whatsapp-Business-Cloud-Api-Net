using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SharedWABAIDResponse
    {
        [JsonProperty("data")]
        public SharedWABAIDData Data { get; set; }
    }

    public class SharedWABAIDData
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }

        [JsonProperty("granular_scopes")]
        public List<GranularScope> GranularScopes { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public partial class GranularScope
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("target_ids")]
        public List<string> TargetIds { get; set; }
    }
}