using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
	public class ProductOrderMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<ProductOrderMessageEntry> Entry { get; set; }
    }

    public class ProductOrderMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<ProductOrderMessageChange> Changes { get; set; }
    }

    public class ProductOrderMessageChange
    {
        [JsonPropertyName("value")]
        public ProductOrderMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class ProductOrderMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public ProductOrderMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<ProductOrderMessageMessage> Messages { get; set; }
    }

    

    public class ProductOrderMessageMessage:GenericMessage
    {    

        [JsonPropertyName("order")]
        public Order Order { get; set; }

        [JsonPropertyName("context")]
        public ProductOrderMessageContext Context { get; set; }
    }

    public class ProductOrderMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
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

    public class ProductOrderMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}