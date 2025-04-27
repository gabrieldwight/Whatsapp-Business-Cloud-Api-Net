using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveTemplateMessageRequest
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
		public string Type { get; private set; } = "template";

        [JsonPropertyName("template")]
        public InteractiveMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public InteractiveMessageLanguage Language { get; set; }

        [JsonPropertyName("components")]
        public List<InteractiveMessageComponent> Components { get; set; }
    }

    public class InteractiveMessageComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<InteractiveMessageParameter> Parameters { get; set; }

        [JsonPropertyName("sub_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Index { get; set; }
    }

    public class InteractiveMessageParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InteractiveMessageImage Image { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

        [JsonPropertyName("currency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InteractiveMessageCurrency Currency { get; set; }

        [JsonPropertyName("date_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InteractiveMessageDateTime DateTime { get; set; }

        [JsonPropertyName("payload")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Payload { get; set; }

        [JsonPropertyName("document")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InteractiveMessageDocument Document { get; set; }

		[JsonPropertyName("video")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InteractiveMessageVideo Video { get; set; }
	}

    public class InteractiveMessageCurrency
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("amount_1000")]
        public long? Amount1000 { get; set; }
    }

    public class InteractiveMessageDateTime
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("day_of_week")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? DayOfWeek { get; set; }

        [JsonPropertyName("year")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Year { get; set; }

        [JsonPropertyName("month")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Month { get; set; }

        [JsonPropertyName("day_of_month")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? DayOfMonth { get; set; }

        [JsonPropertyName("hour")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Hour { get; set; }

        [JsonPropertyName("minute")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Minute { get; set; }

        [JsonPropertyName("calendar")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Calendar { get; set; }
    }

    public class InteractiveMessageImage
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Id { get; set; }

        [JsonPropertyName("link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Link { get; set; }

        [JsonPropertyName("caption")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Caption { get; set; }

		[JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Provider Provider { get; set; }
	}

    public class InteractiveMessageDocument
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Id { get; set; }

        [JsonPropertyName("link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Link { get; set; }

        [JsonPropertyName("filename")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string FileName { get; set; }

		[JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Provider Provider { get; set; }
	}

	public class InteractiveMessageVideo
	{
		[JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Id { get; set; }

		[JsonPropertyName("link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Link { get; set; }

        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Provider Provider { get; set; }
	}

	public class InteractiveMessageLanguage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class Provider
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}