using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ExchangeTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("error")]
        public JsonElement? ErrorElement { get; set; }

        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; set; }

        /// <summary>
        /// Gets the error as a string representation.
        /// Handles both string and object error formats from the API.
        /// </summary>
        [JsonIgnore]
        public string? Error
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
