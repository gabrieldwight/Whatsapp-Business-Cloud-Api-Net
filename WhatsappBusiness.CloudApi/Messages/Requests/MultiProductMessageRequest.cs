using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class MultiProductMessageRequest
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
        public MultiProductInteractive Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MultiProductInteractive
    {
        [JsonPropertyName("type")]
        [JsonInclude]
        public string Type { get; private set; } = "product_list";

        [JsonPropertyName("header")]
        public MultiProductHeader Header { get; set; }

        [JsonPropertyName("body")]
        public MultiProductBody Body { get; set; }

        [JsonPropertyName("footer")]
        public MultiProductBody Footer { get; set; }

        [JsonPropertyName("action")]
        public MultiProductAction Action { get; set; }
    }

    public class MultiProductAction
    {
        [JsonPropertyName("catalog_id")]
        public string CatalogId { get; set; }

        [JsonPropertyName("sections")]
        public List<MultiProductSection> Sections { get; set; }
    }

    public class MultiProductSection
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("product_items")]
        public List<ProductItem> ProductItems { get; set; }
    }

    public class ProductItem
    {
        [JsonPropertyName("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class MultiProductBody
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class MultiProductHeader
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}