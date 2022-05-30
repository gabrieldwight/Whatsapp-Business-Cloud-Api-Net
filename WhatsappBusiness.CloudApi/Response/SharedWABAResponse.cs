using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SharedWABAResponse
    {
        [JsonProperty("data")]
        public List<WABAData> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public class WABAData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("timezone_id")]
        public string TimezoneId { get; set; }

        [JsonProperty("message_template_namespace")]
        public string MessageTemplateNamespace { get; set; }
    }

    public class Paging
    {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }
}