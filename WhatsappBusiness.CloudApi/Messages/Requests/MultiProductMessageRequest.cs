using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class MultiProductMessageRequest
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
        public MultiProductInteractive Interactive { get; set; }
    }

    public class MultiProductInteractive
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "product_list";

        [JsonProperty("header")]
        public MultiProductHeader Header { get; set; }

        [JsonProperty("body")]
        public MultiProductBody Body { get; set; }

        [JsonProperty("footer")]
        public MultiProductBody Footer { get; set; }

        [JsonProperty("action")]
        public MultiProductAction Action { get; set; }
    }

    public class MultiProductAction
    {
        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("sections")]
        public List<MultiProductSection> Sections { get; set; }
    }

    public class MultiProductSection
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("product_items")]
        public List<ProductItem> ProductItems { get; set; }
    }

    public class ProductItem
    {
        [JsonProperty("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class MultiProductBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class MultiProductHeader
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}