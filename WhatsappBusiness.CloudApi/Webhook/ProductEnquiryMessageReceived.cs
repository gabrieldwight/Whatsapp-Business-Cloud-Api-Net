using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{


    public class ProductEnquiryMessage : GenericMessage
    {
        [JsonPropertyName("context")]
        public ProductEnquiryContext Context { get; set; }
        
        [JsonPropertyName("text")]
        public ProductEnquiryText Text { get; set; }

    }

    public class ProductEnquiryContext : MessageContext
    {
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
    
}