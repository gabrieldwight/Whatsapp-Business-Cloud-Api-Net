using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Requests
{
    public class VerifyCodeRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
