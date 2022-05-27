using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Registration.Requests
{
    public class RegisterPhoneRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
}
