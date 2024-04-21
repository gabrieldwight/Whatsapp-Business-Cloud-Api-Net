using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateNamespaceResponse
	{
		[JsonProperty("message_template_namespace")]
		public string MessageTemplateNamespace { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }
	}
}
