using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Calls.Requests
{
	public class CallRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("to")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string To { get; set; }

		[JsonPropertyName("call_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string CallId { get; set; }

		[JsonPropertyName("action")]
		public string Action { get; set; }

		[JsonPropertyName("session")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CallSession Session { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class CallSession
	{
		[JsonPropertyName("sdp_type")]
		[JsonInclude]
		public string SdpType { get; private set; } = "offer";

		[JsonPropertyName("sdp")]
		public string Sdp { get; set; }
	}
}
