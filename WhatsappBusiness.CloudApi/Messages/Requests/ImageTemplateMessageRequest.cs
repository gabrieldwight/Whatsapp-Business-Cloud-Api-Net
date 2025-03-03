using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class ImageTemplateMessageRequest
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
        public ImageMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class ImageMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public ImageMessageLanguage Language { get; set; }

        [JsonPropertyName("components")]
        public List<ImageMessageComponent> Components { get; set; }
    }

    public class ImageMessageComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<ImageMessageParameter> Parameters { get; set; }
    }

    public class ImageMessageParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Image Image { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

        [JsonPropertyName("currency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ImageTemplateCurrency Currency { get; set; }

        [JsonPropertyName("date_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ImageTemplateDateTime DateTime { get; set; }
    }

    public class ImageTemplateCurrency
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("amount_1000")]
        public long? Amount1000 { get; set; }
    }

    public class ImageTemplateDateTime
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

    public class Image
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }
    }

    public class ImageMessageLanguage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}