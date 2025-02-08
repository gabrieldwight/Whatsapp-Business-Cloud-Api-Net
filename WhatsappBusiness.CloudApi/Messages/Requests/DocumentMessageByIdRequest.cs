using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class DocumentMessageByIdRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; private set; } = "document";

        [JsonPropertyName("document")]
        public MediaDocument Document { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MediaDocument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }
    }
}
