using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class BaseCreateTemplateMessageRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("allow_category_change", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowCategoryChange { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("library_template_name", NullValueHandling = NullValueHandling.Ignore)]
        public string? LibraryTemplateName { get; set; }

        [JsonProperty("LIBRARY_TEMPLATE_BUTTON_INPUTS", NullValueHandling = NullValueHandling.Ignore)]
        public List<object>? LibraryTemplateButtonInputs { get; set; }

        [JsonProperty("components")]
        public List<object> Components { get; set; }
    }
}
