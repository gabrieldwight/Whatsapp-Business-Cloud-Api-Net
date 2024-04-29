using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class TemplateBaseResponse
    {
        [JsonProperty("data")]
        public List<TemplateData> Data { get; set; }

        [JsonProperty("paging")]
        public TemplatePaging Paging { get; set; }
    }

    public class TemplateData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("components")]
        public List<object> Components { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class TemplatePaging
    {
        [JsonProperty("cursors")]
        public TemplateCursors Cursors { get; set; }
    }

    public class TemplateCursors
    {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }
}
