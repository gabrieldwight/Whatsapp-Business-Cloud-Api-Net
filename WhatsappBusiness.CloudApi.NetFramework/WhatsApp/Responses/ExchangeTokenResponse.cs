using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Responses
{
    /// <summary>
    /// Response model for token exchange
    /// </summary>
    public class ExchangeTokenResponse
    {
        /// <summary>
        /// Access token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Token type
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Token expiration time in seconds
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Token scope
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Error element for handling both string and object error formats
        /// </summary>
        [JsonPropertyName("error")]
        public JsonElement? ErrorElement { get; set; }

        /// <summary>
        /// Error description
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets the error as a string representation.
        /// Handles both string and object error formats from the API.
        /// </summary>
        [JsonIgnore]
        public string Error
        {
            get
            {
                if (!ErrorElement.HasValue)
                    return null;

                var errorElement = ErrorElement.Value;
                
                if (errorElement.ValueKind == JsonValueKind.String)
                {
                    return errorElement.GetString();
                }
                else if (errorElement.ValueKind == JsonValueKind.Object)
                {
                    // If it's an object, try to get a meaningful error message
                    if (errorElement.TryGetProperty("message", out var messageElement))
                    {
                        return messageElement.GetString();
                    }
                    else if (errorElement.TryGetProperty("type", out var typeElement))
                    {
                        return typeElement.GetString();
                    }
                    else
                    {
                        // Return the raw JSON object as string
                        return errorElement.GetRawText();
                    }
                }
                else
                {
                    // For arrays or other types, return the raw JSON
                    return errorElement.GetRawText();
                }
            }
        }
    }
}