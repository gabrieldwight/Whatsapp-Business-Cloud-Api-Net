using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Responses
{
    /// <summary>
    /// Response model for shared WABA ID information
    /// </summary>
    public class SharedWABAIDResponse
    {
        /// <summary>
        /// Data containing shared WABA information
        /// </summary>
        [JsonPropertyName("data")]
        public SharedWABAIDData Data { get; set; }

        /// <summary>
        /// Gets the shared WABA ID from the response
        /// </summary>
        /// <returns>The shared WABA ID if found, otherwise null</returns>
        public string GetSharedWABAId()
        {
            return Data
                ?.GranularScopes
                ?.FirstOrDefault(x => x.Scope == "whatsapp_business_management" || x.Scope == "whatsapp_business_messaging")
                ?.TargetIds
                ?.FirstOrDefault();
        }
    }

    /// <summary>
    /// Data container for shared WABA ID response
    /// </summary>
    public class SharedWABAIDData
    {
        /// <summary>
        /// App ID
        /// </summary>
        [JsonPropertyName("app_id")]
        public string AppId { get; set; }

        /// <summary>
        /// Token type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        [JsonPropertyName("application")]
        public string Application { get; set; }

        /// <summary>
        /// Data access expiration timestamp
        /// </summary>
        [JsonPropertyName("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }

        /// <summary>
        /// Whether the token is valid
        /// </summary>
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        /// <summary>
        /// List of scopes
        /// </summary>
        [JsonPropertyName("scopes")]
        public List<string> Scopes { get; set; }

        /// <summary>
        /// List of granular scopes with target IDs
        /// </summary>
        [JsonPropertyName("granular_scopes")]
        public List<GranularScope> GranularScopes { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>
    /// Granular scope information
    /// </summary>
    public class GranularScope
    {
        /// <summary>
        /// Scope name
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Target IDs for the scope
        /// </summary>
        [JsonPropertyName("target_ids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> TargetIds { get; set; }
    }
}