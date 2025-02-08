using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class SingleProductMessageRequest
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
        public SingleProductInteractive Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class SingleProductInteractive
    {
        [JsonPropertyName("type")]
        [JsonInclude]
        public string Type { get; private set; } = "product";

        [JsonPropertyName("body")]
        public SingleProductBody Body { get; set; }

        [JsonPropertyName("footer")]
        public SingleProductBody Footer { get; set; }

        [JsonPropertyName("action")]
        public SingleProductAction Action { get; set; }
    }

    public class SingleProductAction
    {
        [JsonPropertyName("catalog_id")]
        public string CatalogId { get; set; }

        [JsonPropertyName("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class SingleProductBody
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}