using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveTemplateMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "template";

        [JsonProperty("template")]
        public InteractiveMessageTemplate Template { get; set; }

		[JsonProperty("biz_opaque_callback_data", NullValueHandling = NullValueHandling.Ignore)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public InteractiveMessageLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<InteractiveMessageComponent> Components { get; set; }
    }

    public class InteractiveMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<InteractiveMessageParameter> Parameters { get; set; }

        [JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SubType { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }
    }

    public class InteractiveMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

		[JsonProperty("parameter_name", NullValueHandling = NullValueHandling.Ignore)]
		public string ParameterName { get; set; }

		[JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageImage Image { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageCurrency Currency { get; set; }

        [JsonProperty("date_time", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageDateTime DateTime { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public string Payload { get; set; }

        [JsonProperty("document", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageDocument Document { get; set; }

		[JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
		public InteractiveMessageVideo Video { get; set; }
	}

    public class InteractiveMessageCurrency
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount_1000")]
        public long Amount1000 { get; set; }
    }

    public class InteractiveMessageDateTime
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("day_of_week", NullValueHandling = NullValueHandling.Ignore)]
        public long DayOfWeek { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public long Year { get; set; }

        [JsonProperty("month", NullValueHandling = NullValueHandling.Ignore)]
        public long Month { get; set; }

        [JsonProperty("day_of_month", NullValueHandling = NullValueHandling.Ignore)]
        public long DayOfMonth { get; set; }

        [JsonProperty("hour", NullValueHandling = NullValueHandling.Ignore)]
        public long Hour { get; set; }

        [JsonProperty("minute", NullValueHandling = NullValueHandling.Ignore)]
        public long Minute { get; set; }

        [JsonProperty("calendar", NullValueHandling = NullValueHandling.Ignore)]
        public string Calendar { get; set; }
    }

    public class InteractiveMessageImage
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public string Link { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

		[JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
		public Provider Provider { get; set; }
	}

    public class InteractiveMessageDocument
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public string Link { get; set; }

        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }

		[JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
		public Provider Provider { get; set; }
	}

	public class InteractiveMessageVideo
	{
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		[JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
		public string Link { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public Provider Provider { get; set; }
	}

	public class InteractiveMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class Provider
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}