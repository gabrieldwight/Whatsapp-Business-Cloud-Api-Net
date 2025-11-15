using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateLibraryResponse
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public string Language { get; set; }

		[JsonPropertyName("category")]
		public string Category { get; set; }

		[JsonPropertyName("topic")]
		public string Topic { get; set; }

		[JsonPropertyName("usecase")]
		public string Usecase { get; set; }

		[JsonPropertyName("industry")]
		public List<string> Industry { get; set; }

		[JsonPropertyName("header")]
		public string Header { get; set; }

		[JsonPropertyName("body")]
		public string Body { get; set; }

		[JsonPropertyName("body_params")]
		public List<string> BodyParams { get; set; }

		[JsonPropertyName("buttons")]
		public List<Button> Buttons { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	public class Button
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
	}
}
