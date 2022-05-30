using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.BusinessProfile.Requests
{
    public class UpdateBusinessProfileRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("vertical")]
        public string Vertical { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("websites")]
        public List<string> Websites { get; set; }

        [JsonProperty("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }
    }
}
