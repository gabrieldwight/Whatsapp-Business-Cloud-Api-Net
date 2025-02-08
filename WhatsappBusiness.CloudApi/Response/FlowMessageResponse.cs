using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class FlowMessageResponse : WhatsAppResponse
	{
		[JsonPropertyName("meta")]
		public Meta Meta { get; set; }
	}

	public class Meta
	{
		[JsonPropertyName("api_status")]
		public string ApiStatus { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }
	}
}
