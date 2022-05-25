using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Requests
{
    public class MarkMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}
