using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateByIdResponse
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("components")]
		public List<object> Components { get; set; }

		[JsonProperty("language")]
		public string Language { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }
	}
}
