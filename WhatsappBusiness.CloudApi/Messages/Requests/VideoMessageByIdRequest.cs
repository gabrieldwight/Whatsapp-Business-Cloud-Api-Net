using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class VideoMessageByIdRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "video";

        [JsonPropertyName("video")]
        public MediaVideo Video { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MediaVideo
    {
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
