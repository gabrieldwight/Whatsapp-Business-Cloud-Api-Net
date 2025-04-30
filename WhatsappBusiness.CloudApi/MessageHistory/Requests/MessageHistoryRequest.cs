using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.MessageHistory.Requests
{
	public class MessageHistoryRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("sync_type")]
		public string SyncType { get; set; }
	}
}
