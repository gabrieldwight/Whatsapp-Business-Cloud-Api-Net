using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
	public class AccountUpdateEvent
	{
		[JsonPropertyName("object")]
		public string Object { get; set; }

		[JsonPropertyName("entry")]
		public List<AccountEntry> Entry { get; set; }
	}

	public class AccountEntry
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("time")]
		public long Time { get; set; }

		[JsonPropertyName("changes")]
		public List<AccountChange> Changes { get; set; }
	}

	public class AccountChange
	{
		[JsonPropertyName("value")]
		public Value Value { get; set; }

		[JsonPropertyName("field")]
		public string Field { get; set; }
	}

	public class Value
	{
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }

		[JsonPropertyName("event")]
		public string Event { get; set; }
	}
}
