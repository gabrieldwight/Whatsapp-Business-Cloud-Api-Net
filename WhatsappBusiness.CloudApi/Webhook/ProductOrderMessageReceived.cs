using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class ProductOrderMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ProductOrderMessageEntry> Entry { get; set; }
    }

    public class ProductOrderMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ProductOrderMessageChange> Changes { get; set; }
    }

    public class ProductOrderMessageChange
    {
        [JsonProperty("value")]
        public ProductOrderMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ProductOrderMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ProductOrderMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ProductOrderMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ProductOrderMessageMessage> Messages { get; set; }
    }

    public class ProductOrderMessageContact
    {
        [JsonProperty("profile")]
        public ProductOrderMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ProductOrderMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductOrderMessageMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order")]
        public Order Order { get; set; }

        [JsonProperty("context")]
        public ProductOrderMessageContext Context { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ProductOrderMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Order
    {
        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("product_items")]
        public List<ProductItem> ProductItems { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ProductItem
    {
        [JsonProperty("product_retailer_id")]
        public string ProductRetailerId { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("item_price")]
        public string ItemPrice { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class ProductOrderMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}