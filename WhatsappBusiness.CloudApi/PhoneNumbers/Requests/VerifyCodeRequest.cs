using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.PhoneNumbers.Requests
{
    public class VerifyCodeRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
