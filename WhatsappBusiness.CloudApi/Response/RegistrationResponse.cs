using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class RegistrationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
