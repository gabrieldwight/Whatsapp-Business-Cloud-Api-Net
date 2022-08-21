using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ResumableUploadResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("file_offset")]
        public long FileOffset { get; set; }

        [JsonProperty("h")]
        public string H { get; set; }
    }
}
