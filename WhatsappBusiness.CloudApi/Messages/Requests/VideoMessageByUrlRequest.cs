﻿using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class VideoMessageByUrlRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "video";

        [JsonProperty("video")]
        public MediaVideoUrl Video { get; set; }

		[JsonProperty("biz_opaque_callback_data", NullValueHandling = NullValueHandling.Ignore)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class MediaVideoUrl
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }
}
