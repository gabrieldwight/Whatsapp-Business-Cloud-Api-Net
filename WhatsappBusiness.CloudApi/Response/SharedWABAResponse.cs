using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SharedWABAResponse
    {
        [JsonPropertyName("data")]
        public List<WABAData> Data { get; set; }

        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }
    }

    public class WABAData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("timezone_id")]
        public string TimezoneId { get; set; }

        [JsonPropertyName("message_template_namespace")]
        public string MessageTemplateNamespace { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }
}