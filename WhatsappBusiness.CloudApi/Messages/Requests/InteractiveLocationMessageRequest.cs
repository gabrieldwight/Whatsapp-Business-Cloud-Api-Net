using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class InteractiveLocationMessageRequest
	{
		[JsonProperty("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonProperty("recipient_type")]
		public string RecipientType { get; private set; } = "individual";

		[JsonProperty("type")]
		public string Type { get; private set; } = "interactive";

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("interactive")]
		public InteractiveLocationRequestMessage Interactive { get; set; }
	}

	public class InteractiveLocationRequestMessage
	{
		[JsonProperty("type")]
		public string Type { get; private set; } = "location_request_message";

		[JsonProperty("body")]
		public InteractiveLocationBody Body { get; set; }

		[JsonProperty("action")]
		public InteractiveLocationAction Action { get; set; }
	}

	public class InteractiveLocationAction
	{
		[JsonProperty("name")]
		public string Name { get; private set; } = "send_location";
	}

	public class InteractiveLocationBody
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
