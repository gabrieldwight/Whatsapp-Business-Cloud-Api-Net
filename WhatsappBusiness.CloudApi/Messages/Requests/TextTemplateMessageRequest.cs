using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class TextTemplateMessageRequest
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
        public TextMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class TextMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public TextMessageLanguage Language { get; set; }

        [JsonPropertyName("components")]
        public List<TextMessageComponent> Components { get; set; }
    }

    public class TextMessageComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<TextMessageParameter> Parameters { get; set; }

        [JsonPropertyName("sub_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Index { get; set; }
    }

    public class TextMessageParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameter_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

        [JsonPropertyName("image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TemplateImage Image { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

        [JsonPropertyName("currency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TemplateCurrency Currency { get; set; }

        [JsonPropertyName("date_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TemplateDateTime DateTime { get; set; }

        [JsonPropertyName("payload")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Payload { get; set; }
    }

    public class TemplateCurrency
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("amount_1000")]
        public long? Amount1000 { get; set; }
    }

    public class TemplateDateTime
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("day_of_week")]
        public long? DayOfWeek { get; set; }

        [JsonPropertyName("year")]
        public long? Year { get; set; }

        [JsonPropertyName("month")]
        public long? Month { get; set; }

        [JsonPropertyName("day_of_month")]
        public long? DayOfMonth { get; set; }

        [JsonPropertyName("hour")]
        public long? Hour { get; set; }

        [JsonPropertyName("minute")]
        public long? Minute { get; set; }

        [JsonPropertyName("calendar")]
        public string Calendar { get; set; }
    }

    public class TemplateImage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }
    }

    public class TextMessageLanguage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
