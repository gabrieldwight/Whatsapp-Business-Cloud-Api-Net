using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class VoiceCallMessageRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "interactive";

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("interactive")]
		public InteractiveVoiceCallRequestMessage Interactive { get; set; }
	}

	public class InteractiveVoiceCallRequestMessage
	{
		[JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "voice_call";

		[JsonPropertyName("body")]
		public InteractiveVoiceCallBody Body { get; set; }

		[JsonPropertyName("action")]
		public InteractiveVoiceCallAction Action { get; set; }
	}

	public class InteractiveVoiceCallBody
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class InteractiveVoiceCallAction
	{
		[JsonPropertyName("name")]
		[JsonInclude]
		public string Name { get; private set; } = "voice_call";

		[JsonPropertyName("parameters")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public VoiceCallParameters Parameters { get; set; }
	}

	public class VoiceCallParameters
	{
		[JsonPropertyName("display_text")]
		public string DisplayText { get; set; }

		[JsonPropertyName("ttl_minutes")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int TtlMinutes { get; set; } // Duration in minutes
	}
}
