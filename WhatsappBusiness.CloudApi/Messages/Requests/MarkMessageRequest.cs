using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class MarkMessageRequest
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("message_id")]
		public string MessageId { get; set; }

		[JsonPropertyName("typing_indicator")]
		public TypingIndicator TypingIndicator { get; set; }
	}

	public class TypingIndicator
	{
		[JsonPropertyName("type")]
		public string Type { get; private set; } = "text";
	}
}