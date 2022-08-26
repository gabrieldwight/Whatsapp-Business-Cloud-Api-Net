using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class ProductEnquiryMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ProductEnquiryEntry> Entry { get; set; }
    }

    public class ProductEnquiryEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ProductEnquiryChange> Changes { get; set; }
    }

    public class ProductEnquiryChange
    {
        [JsonProperty("value")]
        public ProductEnquiryValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ProductEnquiryValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ProductEnquiryMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ProductEnquiryContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ProductEnquiryMessage> Messages { get; set; }
    }

    public class ProductEnquiryContact
    {
        [JsonProperty("profile")]
        public ProductEnquiryProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ProductEnquiryProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductEnquiryMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public ProductEnquiryText Text { get; set; }

        [JsonProperty("context")]
        public ProductEnquiryContext Context { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ProductEnquiryContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("referred_product")]
        public ReferredProduct ReferredProduct { get; set; }
    }

    public partial class ReferredProduct
    {
        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("product_retailer_id")]
        public string ProductRetailerId { get; set; }
    }

    public class ProductEnquiryText
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class ProductEnquiryMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}