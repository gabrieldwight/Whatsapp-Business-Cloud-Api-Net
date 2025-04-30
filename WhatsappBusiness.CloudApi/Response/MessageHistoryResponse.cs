using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class MessageHistoryResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("request_id")]
		public string RequestId { get; set; }
	}
}
