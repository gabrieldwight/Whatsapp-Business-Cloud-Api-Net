using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class WhatsAppCallResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("calls")]
		public List<WhatsAppCall> Calls { get; set; }
	}

	public class WhatsAppCall
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
