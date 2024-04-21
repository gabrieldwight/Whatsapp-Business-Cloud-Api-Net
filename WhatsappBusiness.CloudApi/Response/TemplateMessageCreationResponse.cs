using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateMessageCreationResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("category")]
		public string Category { get; set; }
	}
}
