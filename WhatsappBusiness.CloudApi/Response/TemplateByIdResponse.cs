using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateByIdResponse
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("components")]
		public List<object> Components { get; set; }

		[JsonPropertyName("language")]
		public string Language { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("category")]
		public string Category { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
