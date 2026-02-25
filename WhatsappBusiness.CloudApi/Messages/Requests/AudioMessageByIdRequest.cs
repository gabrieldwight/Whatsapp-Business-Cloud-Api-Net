using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class AudioMessageByIdRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
        [JsonInclude]
        public string RecipientType { get; set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

		[JsonPropertyName("recipient")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Recipient { get; set; }

		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "audio";

        [JsonPropertyName("audio")]
        public MediaAudio Audio { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MediaAudio
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

		[JsonPropertyName("voice")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool Voice { get; set; }
	}
}
