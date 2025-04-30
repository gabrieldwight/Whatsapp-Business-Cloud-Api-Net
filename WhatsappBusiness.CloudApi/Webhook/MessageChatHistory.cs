using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
	public class MessageChatHistory
	{
		[JsonPropertyName("object")]
		public string Object { get; set; }

		[JsonPropertyName("entry")]
		public List<MessageChatEntry> Entry { get; set; }
	}

	public class MessageChatEntry
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("changes")]
		public List<MessageChatChange> Changes { get; set; }
	}

	public class MessageChatChange
	{
		[JsonPropertyName("value")]
		public MessageChatValue Value { get; set; }

		[JsonPropertyName("field")]
		public string Field { get; set; }
	}

	public class MessageChatValue
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("metadata")]
		public ValueMetadata Metadata { get; set; }

		[JsonPropertyName("history")]
		public List<History> History { get; set; }
	}

	public class History
	{
		[JsonPropertyName("metadata")]
		public HistoryMetadata Metadata { get; set; }

		[JsonPropertyName("threads")]
		public List<Thread> Threads { get; set; }
	}

	public class HistoryMetadata
	{
		[JsonPropertyName("phase")]
		public long Phase { get; set; }

		[JsonPropertyName("chunk_order")]
		public long ChunkOrder { get; set; }

		[JsonPropertyName("progress")]
		public long Progress { get; set; }
	}

	public partial class Thread
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("messages")]
		public List<Message> Messages { get; set; }
	}

	public class Message
	{
		[JsonPropertyName("from")]
		public string From { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Text Text { get; set; }

		[JsonPropertyName("history_context")]
		public HistoryContext HistoryContext { get; set; }
	}

	public partial class HistoryContext
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }
	}

	public partial class Text
	{
		[JsonPropertyName("body")]
		public string Body { get; set; }
	}

	public partial class ValueMetadata
	{
		[JsonPropertyName("display_phone_number")]
		public string DisplayPhoneNumber { get; set; }

		[JsonPropertyName("phone_number_id")]
		public string PhoneNumberId { get; set; }
	}
}
