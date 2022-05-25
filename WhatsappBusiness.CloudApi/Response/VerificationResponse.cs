using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class VerificationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
