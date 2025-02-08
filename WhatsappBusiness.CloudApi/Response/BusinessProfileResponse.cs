using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class BusinessProfileResponse
    {
        [JsonPropertyName("data")]
        public List<BusinessProfileData> Data { get; set; }
    }

    public class BusinessProfileData
    {
        [JsonPropertyName("business_profile")]
        public BusinessProfile BusinessProfile { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class BusinessProfile
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vertical")]
        public string Vertical { get; set; }

        [JsonPropertyName("about")]
        public string About { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("websites")]
        public List<string> Websites { get; set; }

        [JsonPropertyName("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}