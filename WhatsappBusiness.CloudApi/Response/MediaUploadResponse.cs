using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class MediaUploadResponse
    {
        [JsonProperty("id")]
        public string MediaId { get; set; }
    }
}
