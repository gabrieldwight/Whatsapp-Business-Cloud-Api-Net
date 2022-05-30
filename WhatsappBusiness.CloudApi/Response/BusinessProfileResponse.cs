using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class BusinessProfileResponse
    {
        [JsonProperty("data")]
        public List<BusinessProfileData> Data { get; set; }
    }

    public class BusinessProfileData
    {
        [JsonProperty("business_profile")]
        public BusinessProfile BusinessProfile { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class BusinessProfile
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("vertical")]
        public string Vertical { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("websites")]
        public List<string> Websites { get; set; }

        [JsonProperty("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}