using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class TextMessageRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "text";

        [JsonPropertyName("text")]
        public WhatsAppText Text { get; set; }

        [JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class WhatsAppText
    {
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
