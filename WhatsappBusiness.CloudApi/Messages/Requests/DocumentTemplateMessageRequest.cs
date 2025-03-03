using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class DocumentTemplateMessageRequest
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
        public DocumentMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class DocumentMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public DocumentMessageLanguage Language { get; set; }

        [JsonPropertyName("components")]
        public List<DocumentMessageComponent> Components { get; set; }
    }

    public class DocumentMessageComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<DocumentMessageParameter> Parameters { get; set; }
    }

    public class DocumentMessageParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("document")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Document Document { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

        [JsonPropertyName("currency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public DocumentTemplateCurrency Currency { get; set; }

        [JsonPropertyName("date_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public DocumentTemplateDateTime DateTime { get; set; }
    }

    public class DocumentTemplateCurrency
    {
        [JsonPropertyName("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("amount_1000")]
        public long? Amount1000 { get; set; }
    }

    public class DocumentTemplateDateTime
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

    public class Document
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }
    }

    public class DocumentMessageLanguage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
