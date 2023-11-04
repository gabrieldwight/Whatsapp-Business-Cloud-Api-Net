using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class FlowTemplateMessageRequest
	{
		[JsonProperty("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonProperty("recipient_type")]
		public string RecipientType { get; private set; } = "individual";

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("type")]
		public string Type { get; private set; } = "template";

		[JsonProperty("template")]
		public FlowMessageTemplate Template { get; set; }
	}

	public class FlowMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public FlowMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<FlowMessageComponent> Components { get; set; }
	}

	public class FlowMessageComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<FlowTemplateMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }
	}

	public class FlowTemplateMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
		public FlowTemplateMessageAction Action { get; set; }
	}

	public class FlowTemplateMessageAction
	{
		[JsonProperty("flow_token")]
		public string FlowToken { get; set; }

		[JsonProperty("flow_action_data")]
		public object FlowActionData { get; set; }
	}

	public class FlowMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
