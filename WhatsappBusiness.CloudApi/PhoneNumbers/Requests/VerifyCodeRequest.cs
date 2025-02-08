using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.PhoneNumbers.Requests
{
    public class VerifyCodeRequest
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
