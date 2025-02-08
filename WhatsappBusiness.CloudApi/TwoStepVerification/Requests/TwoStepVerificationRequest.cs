using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.TwoStepVerification.Requests
{
    public class TwoStepVerificationRequest
    {
        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
}
