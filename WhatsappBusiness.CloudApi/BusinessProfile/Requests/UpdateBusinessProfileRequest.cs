using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.BusinessProfile.Requests
{
    public class UpdateBusinessProfileRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vertical")]
        public string Vertical { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("websites")]
        public List<string> Websites { get; set; }

		[JsonPropertyName("profile_picture_handle")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ProfilePictureHandle { get; set; }
	}
}
