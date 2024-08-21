using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class WhatsAppBusinessHSMWhatsAppHSMComponentGet
    {
        [JsonProperty("add_security_recommendation")]
        public bool AddSecurityRecommendation { get; set; }

        [JsonProperty("code_expiration_minutes")]
        public int CodeExpirationMinutes { get; set; }

        [JsonProperty("example")]
        public object Example { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
