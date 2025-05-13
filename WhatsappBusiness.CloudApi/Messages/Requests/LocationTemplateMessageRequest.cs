using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class LocationTemplateMessageRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "template";

        [JsonPropertyName("template")]
        public LocationMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class LocationMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("language")]
        public LocationMessageLanguage Language { get; set; }

        [JsonPropertyName("components")]
        public List<LocationMessageComponent> Components { get; set; }
    }

    public class LocationMessageComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("parameters")]
        public List<LocationMessageParameter> Parameters { get; set; }
    }

    public class LocationMessageParameter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public LocationDetails Location { get; set; }

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }
    }

    public class LocationDetails
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class LocationMessageLanguage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
