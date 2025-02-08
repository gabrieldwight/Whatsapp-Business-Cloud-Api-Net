using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class FlowMessageRequest
	{
		[JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "interactive";

		[JsonPropertyName("interactive")]
		public FlowMessageInteractive Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class FlowMessageInteractive
	{
		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "flow";

		[JsonPropertyName("header")]
		public FlowMessageHeader Header { get; set; }

		[JsonPropertyName("body")]
		public FlowMessageBody Body { get; set; }

		[JsonPropertyName("footer")]
		public FlowMessageFooter Footer { get; set; }

		[JsonPropertyName("action")]
		public FlowMessageAction Action { get; set; }
	}

	public class FlowMessageAction
	{
		[JsonPropertyName("name")]
		[JsonInclude]
		public string Name { get; private set; } = "flow";

		[JsonPropertyName("parameters")]
		public FlowMessageParameters Parameters { get; set; }
	}

	public class FlowMessageParameters
	{
		[JsonPropertyName("flow_message_version")]
		[JsonInclude]
		public string FlowMessageVersion { get; private set; } = "3";

		[JsonPropertyName("flow_token")]
		public string FlowToken { get; set; }

		[JsonPropertyName("flow_id")]
		public string FlowId { get; set; }

		[JsonPropertyName("flow_cta")]
		public string FlowCta { get; set; }

		[JsonPropertyName("flow_action")]
		public string FlowAction { get; set; }

		[JsonPropertyName("flow_action_payload")]
		public FlowActionPayload FlowActionPayload { get; set; }

		[JsonIgnore]
		public bool IsInDraftMode { get; set; }

		[JsonPropertyName("mode")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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
		[JsonPropertyName("screen")]
		public string Screen { get; set; }

		[JsonPropertyName("data")]
		public object Data { get; set; }
	}

	public class FlowMessageBody
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class FlowMessageFooter
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class FlowMessageHeader
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}
}
