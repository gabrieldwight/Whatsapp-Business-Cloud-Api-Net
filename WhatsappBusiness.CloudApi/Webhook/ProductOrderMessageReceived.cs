using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{

    public class ProductOrderMessageMessage:GenericMessage
    {    

        [JsonPropertyName("order")]
        public Order Order { get; set; }

        [JsonPropertyName("context")]
        public MessageContext Context { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("catalog_id")]
        public string CatalogId { get; set; }

        [JsonPropertyName("product_items")]
        public List<ProductItem> ProductItems { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class ProductItem
    {
        [JsonPropertyName("product_retailer_id")]
        public string ProductRetailerId { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("item_price")]
        public string ItemPrice { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
    
}