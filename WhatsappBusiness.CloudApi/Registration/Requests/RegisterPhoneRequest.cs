using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Registration.Requests
{
    public class RegisterPhoneRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
}
