using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.PhoneNumbers.Requests
{
    public class RequestVerificationCode
    {
        [JsonProperty("code_method")]
        public string CodeMethod { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}
