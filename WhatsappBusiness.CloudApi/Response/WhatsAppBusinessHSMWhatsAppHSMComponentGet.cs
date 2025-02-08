using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class WhatsAppBusinessHSMWhatsAppHSMComponentGet
    {
        [JsonPropertyName("add_security_recommendation")]
        public bool AddSecurityRecommendation { get; set; }

        [JsonPropertyName("code_expiration_minutes")]
        public int CodeExpirationMinutes { get; set; }

        [JsonPropertyName("example")]
        public object Example { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

		[JsonPropertyName("buttons")]
		public List<TemplateButton> Buttons { get; set; }
	}

    public class TemplateButton
    {
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }  // For URL type buttons

		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }  // For PHONE_NUMBER type buttons
	}
}
