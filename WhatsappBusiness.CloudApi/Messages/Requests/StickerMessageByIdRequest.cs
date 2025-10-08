﻿using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class StickerMessageByIdRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "sticker";

        [JsonPropertyName("sticker")]
        public MediaSticker Sticker { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MediaSticker
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

		[JsonPropertyName("animated")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool Animated { get; set; }
	}
}
