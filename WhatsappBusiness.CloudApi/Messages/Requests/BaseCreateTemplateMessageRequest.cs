using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class BaseCreateTemplateMessageRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("allow_category_change")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool? AllowCategoryChange { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("library_template_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? LibraryTemplateName { get; set; }

        [JsonPropertyName("LIBRARY_TEMPLATE_BUTTON_INPUTS")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<object>? LibraryTemplateButtonInputs { get; set; }

        [JsonPropertyName("components")]
        public List<object> Components { get; set; }
    }
}
