using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class TemplateBaseResponse
    {
        [JsonPropertyName("data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<TemplateData> Data { get; set; }

        [JsonPropertyName("paging")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TemplatePaging Paging { get; set; }
    }

    public class TemplateData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("components")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<WhatsAppBusinessHSMWhatsAppHSMComponentGet> Components { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Status { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

		[JsonPropertyName("topic")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Topic { get; set; }

		[JsonPropertyName("usecase")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Usecase { get; set; }

		[JsonPropertyName("industry")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> Industry { get; set; }

		[JsonPropertyName("header")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Header { get; set; }

		[JsonPropertyName("body")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Body { get; set; }

		[JsonPropertyName("body_params")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> BodyParams { get; set; }

		[JsonPropertyName("body_param_types")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> BodyParamTypes { get; set; }

		[JsonPropertyName("buttons")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<TemplateLibraryButton> Buttons { get; set; }

		[JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class TemplatePaging
    {
        [JsonPropertyName("cursors")]
        public TemplateCursors Cursors { get; set; }

        [JsonPropertyName("next")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Next { get; set; }
	}

    public class TemplateCursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }

	public class TemplateLibraryButton
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
	}
}
