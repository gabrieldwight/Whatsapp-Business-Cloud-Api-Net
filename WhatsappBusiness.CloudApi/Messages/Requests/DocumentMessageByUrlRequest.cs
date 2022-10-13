using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class DocumentMessageByUrlRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "document";

        [JsonProperty("document")]
        public MediaDocumentUrl Document { get; set; }
    }

    public class MediaDocumentUrl
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }
    }
}
