using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SubscribedAppsResponse
    {
        [JsonPropertyName("data")]
        public List<SubscribedAppsData> Data { get; set; }
    }

    public class SubscribedAppsData
    {
        [JsonPropertyName("whatsapp_business_api_data")]
        public WhatsappBusinessApiData WhatsappBusinessApiData { get; set; }
    }

    public partial class WhatsappBusinessApiData
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
