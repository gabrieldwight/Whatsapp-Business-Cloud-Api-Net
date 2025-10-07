using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Groups.Requests
{
	public class PinGroupMessageRequest
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("recipient_type")]
		public string RecipientType { get; private set; } = "group";

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("pin")]
		public Pin Pin { get; set; }
	}

	public class Pin
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("message_id")]
		public string MessageId { get; set; }

		[JsonPropertyName("expiration_days")]
		public string ExpirationDays { get; set; }
	}
}
