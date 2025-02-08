using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveCTAButtonMessageRequest
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
		public string Type { get; private set; } = "interactive";

        [JsonPropertyName("interactive")]
        public InteractiveCTAButtonMessage Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveCTAButtonMessage
    {
        [JsonPropertyName("type")]
        [JsonInclude]
		public string Type { get; private set; } = "cta_url";

        [JsonPropertyName("header")]
        public CTAButtonHeader Header { get; set; }

        [JsonPropertyName("body")]
        public CTAButtonBody Body { get; set; }

        [JsonPropertyName("footer")]
        public CTAButtonFooter Footer { get; set; }

        [JsonPropertyName("action")]
        public CTAButtonAction Action { get; set; }
    }

    public class CTAButtonAction
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parameters")]
        public CTAButtonParameters Parameters { get; set; }
    }

    public class CTAButtonParameters
    {
        [JsonPropertyName("display_text")]
        public string DisplayText { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class CTAButtonHeader
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class CTAButtonBody
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class CTAButtonFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
