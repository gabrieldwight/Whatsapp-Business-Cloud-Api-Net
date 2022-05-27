using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.TwoStepVerification.Requests
{
    public class TwoStepVerificationRequest
    {
        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
}
