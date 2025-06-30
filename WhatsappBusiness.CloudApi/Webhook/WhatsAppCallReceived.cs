using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
	public class WhatsAppCallReceived
	{
		[JsonPropertyName("entry")]
		public List<WhatsAppCallEntry> Entry { get; set; }

		[JsonPropertyName("object")]
		public string Object { get; set; }
	}

	public class WhatsAppCall
	{
		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }

		[JsonPropertyName("session")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Session Session { get; set; }

		[JsonPropertyName("from")]
		public string From { get; set; }

		[JsonPropertyName("connection")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Connection Connection { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("event")]
		public string Event { get; set; }

		[JsonPropertyName("timestamp")]
		public string Timestamp { get; set; }

		[JsonPropertyName("direction")]
		public string Direction { get; set; }

		[JsonPropertyName("status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Status { get; set; }

		[JsonPropertyName("start_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string StartTime { get; set; }

		[JsonPropertyName("end_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string EndTime { get; set; }

		[JsonPropertyName("duration")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int Duration { get; set; }
	}

	public class Change
	{
		[JsonPropertyName("field")]
		public string Field { get; set; }

		[JsonPropertyName("value")]
		public WhatsAppCallValue Value { get; set; }
	}

	public class Connection
	{
		[JsonPropertyName("webrtc")]
		public Webrtc Webrtc { get; set; }
	}

	public class WhatsAppCallEntry
	{
		[JsonPropertyName("changes")]
		public List<Change> Changes { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	public class Metadata
	{
		[JsonPropertyName("phone_number_id")]
		public string PhoneNumberId { get; set; }

		[JsonPropertyName("display_phone_number")]
		public string DisplayPhoneNumber { get; set; }
	}

	public class Session
	{
		[JsonPropertyName("sdp_type")]
		public string SdpType { get; set; }

		[JsonPropertyName("sdp")]
		public string Sdp { get; set; }
	}

	public class WhatsAppCallValue
	{
		[JsonPropertyName("calls")]
		public List<WhatsAppCall> Calls { get; set; }

		[JsonPropertyName("errors")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<WhatsAppCallError> Errors { get; set; }

		[JsonPropertyName("metadata")]
		public Metadata Metadata { get; set; }

		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }
	}

	public class WhatsAppCallError
	{
		[JsonPropertyName("code")]
		public int Code { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("href")]
		public string Href { get; set; }

		[JsonPropertyName("error_data")]
		public WhatsAppCallErrorData ErrorData { get; set; }
	}

	public class WhatsAppCallErrorData
	{
		[JsonPropertyName("details")]
		public string Details { get; set; }
	}

	public class Webrtc
	{
		[JsonPropertyName("sdp")]
		public string Sdp { get; set; }
	}
}
