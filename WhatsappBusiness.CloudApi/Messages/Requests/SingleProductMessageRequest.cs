using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class SingleProductMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "interactive";

        [JsonProperty("interactive")]
        public SingleProductInteractive Interactive { get; set; }
    }

    public class SingleProductInteractive
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "product";

        [JsonProperty("body")]
        public SingleProductBody Body { get; set; }

        [JsonProperty("footer")]
        public SingleProductBody Footer { get; set; }

        [JsonProperty("action")]
        public SingleProductAction Action { get; set; }
    }

    public class SingleProductAction
    {
        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class SingleProductBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}