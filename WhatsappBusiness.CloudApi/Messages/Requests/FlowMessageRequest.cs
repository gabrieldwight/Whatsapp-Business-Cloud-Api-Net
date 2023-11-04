using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class FlowMessageRequest
	{
		[JsonProperty("recipient_type")]
		public string RecipientType { get; private set; } = "individual";

		[JsonProperty("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("type")]
		public string Type { get; private set; } = "interactive";

		[JsonProperty("interactive")]
		public FlowMessageInteractive Interactive { get; set; }
	}

	public class FlowMessageInteractive
	{
		[JsonProperty("type")]
		public string Type { get; private set; } = "flow";

		[JsonProperty("header")]
		public FlowMessageHeader Header { get; set; }

		[JsonProperty("body")]
		public FlowMessageBody Body { get; set; }

		[JsonProperty("footer")]
		public FlowMessageFooter Footer { get; set; }

		[JsonProperty("action")]
		public FlowMessageAction Action { get; set; }
	}

	public class FlowMessageAction
	{
		[JsonProperty("name")]
		public string Name { get; private set; } = "flow";

		[JsonProperty("parameters")]
		public FlowMessageParameters Parameters { get; set; }
	}

	public class FlowMessageParameters
	{
		[JsonProperty("flow_message_version")]
		public string FlowMessageVersion { get; private set; } = "3";

		[JsonProperty("flow_token")]
		public string FlowToken { get; set; }

		[JsonProperty("flow_id")]
		public string FlowId { get; set; }

		[JsonProperty("flow_cta")]
		public string FlowCta { get; set; }

		[JsonProperty("flow_action")]
		public string FlowAction { get; set; }

		[JsonProperty("flow_action_payload")]
		public FlowActionPayload FlowActionPayload { get; set; }

		[JsonIgnore]
		public bool IsInDraftMode { get; set; }

		[JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
		public string Mode { get; set; }

		public bool ShouldSerializeMode()
		{
			// Only to be used for sending flows that have draft mode do not set this property when sending published flow messages
			return IsInDraftMode;
		}

		public bool ShouldSerializeFlowActionPayload()
		{
			return FlowAction.Equals("navigate", System.StringComparison.OrdinalIgnoreCase);
		}
	}

	public class FlowActionPayload
	{
		[JsonProperty("screen")]
		public string Screen { get; set; }

		[JsonProperty("data")]
		public object Data { get; set; }
	}

	public class FlowMessageBody
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class FlowMessageFooter
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class FlowMessageHeader
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
