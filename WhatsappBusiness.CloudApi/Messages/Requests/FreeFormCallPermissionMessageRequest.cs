using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class FreeFormCallPermissionMessageRequest
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
		public InteractiveCallPermissionRequestMessage Interactive { get; set; }
	}

	public class InteractiveCallPermissionRequestMessage
	{
		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "call_permission_request";
		[JsonPropertyName("body")]
		public InteractiveCallPermissionBody Body { get; set; }
		[JsonPropertyName("action")]
		public InteractiveCallPermissionAction Action { get; set; }
	}

	public class InteractiveCallPermissionBody
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class InteractiveCallPermissionAction
	{
		[JsonPropertyName("name")]
		[JsonInclude]
		public string Name { get; private set; } = "call_permission_request";
	}
}
