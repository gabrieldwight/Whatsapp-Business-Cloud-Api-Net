using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
	public class FlowMessageResponse : WhatsAppResponse
	{
		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}

	public class Meta
	{
		[JsonProperty("api_status")]
		public string ApiStatus { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }
	}
}
