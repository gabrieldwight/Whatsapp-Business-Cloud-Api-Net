using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class InteractiveLocationMessageRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "interactive";

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("interactive")]
		public InteractiveLocationRequestMessage Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class InteractiveLocationRequestMessage
	{
		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "location_request_message";

		[JsonPropertyName("body")]
		public InteractiveLocationBody Body { get; set; }

		[JsonPropertyName("action")]
		public InteractiveLocationAction Action { get; set; }
	}

	public class InteractiveLocationAction
	{
		[JsonPropertyName("name")]
		[JsonInclude]
		public string Name { get; private set; } = "send_location";
	}

	public class InteractiveLocationBody
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}
}
