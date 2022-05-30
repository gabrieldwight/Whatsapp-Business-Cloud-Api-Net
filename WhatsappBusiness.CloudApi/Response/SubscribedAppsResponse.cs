using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class SubscribedAppsResponse
    {
        [JsonProperty("data")]
        public List<SubscribedAppsData> Data { get; set; }
    }

    public class SubscribedAppsData
    {
        [JsonProperty("whatsapp_business_api_data")]
        public WhatsappBusinessApiData WhatsappBusinessApiData { get; set; }
    }

    public partial class WhatsappBusinessApiData
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
