using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class BaseSuccessResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
