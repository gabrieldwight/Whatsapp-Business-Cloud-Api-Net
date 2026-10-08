using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class PhoneNumberByIdResponse
    {
        [JsonPropertyName("verified_name")]
        public string VerifiedName { get; set; }

        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("quality_rating")]
        public string QualityRating { get; set; }

        [JsonPropertyName("whatsapp_business_manager_messaging_limit")]
        public string? WhatsAppBusinessManagerMessagingLimit { get; set; }

        [JsonPropertyName("messaging_limit_tier")]
        public string? MessagingLimitTier { get; set; }
    }
}
