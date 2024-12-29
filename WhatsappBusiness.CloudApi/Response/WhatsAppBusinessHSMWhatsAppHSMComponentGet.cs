using Newtonsoft.Json;
using System.Collections.Generic;

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

		[JsonProperty("buttons")]
		public List<TemplateButton> Buttons { get; set; }
	}

    public class TemplateButton
    {
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }  // For URL type buttons

		[JsonProperty("phone_number")]
		public string PhoneNumber { get; set; }  // For PHONE_NUMBER type buttons
	}
}
