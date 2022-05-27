using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class TextMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "text";

        [JsonProperty("text")]
        public WhatsAppText Text { get; set; }
    }

    public class WhatsAppText
    {
        [JsonProperty("preview_url")]
        public bool PreviewUrl { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
