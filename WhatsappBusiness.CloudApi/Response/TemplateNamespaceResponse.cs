using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateNamespaceResponse
	{
		[JsonPropertyName("message_template_namespace")]
		public string MessageTemplateNamespace { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
