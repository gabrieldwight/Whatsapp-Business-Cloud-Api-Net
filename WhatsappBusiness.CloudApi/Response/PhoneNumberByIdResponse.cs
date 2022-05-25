using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class PhoneNumberByIdResponse
    {
        [JsonProperty("verified_name")]
        public string VerifiedName { get; set; }

        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quality_rating")]
        public string QualityRating { get; set; }
    }
}
