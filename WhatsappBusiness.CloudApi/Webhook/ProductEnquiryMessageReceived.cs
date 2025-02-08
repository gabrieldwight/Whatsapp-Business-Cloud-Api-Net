using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class ProductEnquiryMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<ProductEnquiryEntry> Entry { get; set; }
    }

    public class ProductEnquiryEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<ProductEnquiryChange> Changes { get; set; }
    }

    public class ProductEnquiryChange
    {
        [JsonPropertyName("value")]
        public ProductEnquiryValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class ProductEnquiryValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public ProductEnquiryMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<ProductEnquiryContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<ProductEnquiryMessage> Messages { get; set; }
    }

    public class ProductEnquiryContact
    {
        [JsonPropertyName("profile")]
        public ProductEnquiryProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class ProductEnquiryProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ProductEnquiryMessage
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("text")]
        public ProductEnquiryText Text { get; set; }

        [JsonPropertyName("context")]
        public ProductEnquiryContext Context { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class ProductEnquiryContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("referred_product")]
        public ReferredProduct ReferredProduct { get; set; }
    }

    public partial class ReferredProduct
    {
        [JsonPropertyName("catalog_id")]
        public string CatalogId { get; set; }

        [JsonPropertyName("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class ProductEnquiryText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class ProductEnquiryMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}