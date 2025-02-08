using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class TemplateBaseResponse
    {
        [JsonPropertyName("data")]
        public List<TemplateData> Data { get; set; }

        [JsonPropertyName("paging")]
        public TemplatePaging Paging { get; set; }
    }

    public class TemplateData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("components")]
        public List<WhatsAppBusinessHSMWhatsAppHSMComponentGet> Components { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class TemplatePaging
    {
        [JsonPropertyName("cursors")]
        public TemplateCursors Cursors { get; set; }
    }

    public class TemplateCursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }
}
