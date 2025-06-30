using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CallPermissionTemplateMessageRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "template";

		[JsonPropertyName("template")]
		public CallPermissionMessageTemplate Template { get; set; }
	}

	public class CallPermissionMessageTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public CallPermissionMessageLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<CallPermissionMessageComponent> Components { get; set; }
	}

	public class CallPermissionMessageComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		public List<CallPermissionMessageParameter> Parameters { get; set; }
	}

	public class CallPermissionMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }
	}

	public class CallPermissionMessageLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}
