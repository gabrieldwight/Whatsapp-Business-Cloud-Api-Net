using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
	/// <summary>
	/// The WhatsApp Business API sends notifications to inform you of the status of the messages between you and users. When a message is sent successfully, you receive a notification when the message is sent, delivered, and read. The order of these notifications in your app may not reflect the actual timing of the message status. You can view the timestamp to determine the timing.
	/// </summary>
	public class MessageStatusUpdateNotification
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<MessageStatusUpdateEntry> Entry { get; set; }
    }

    public class MessageStatusUpdateEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<MessageStatusUpdateChange> Changes { get; set; }
    }

    public class MessageStatusUpdateChange
    {
        [JsonPropertyName("value")]
        public MessageStatusUpdateValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class MessageStatusUpdateValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public MessageMetadata Metadata { get; set; }

        [JsonPropertyName("statuses")]
        public List<MessageStatus> Statuses { get; set; }
    }
    
    public class MessageStatus
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; }

		[JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<FailedMessageError> Errors { get; set; }

		[JsonPropertyName("conversation")]
		public MessageConversation Conversation { get; set; }

		[JsonPropertyName("pricing")]
		public MessagePricing Pricing { get; set; }
	}

    public class MessageConversation
    {
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("expiration_timestamp")]
		public string ExpirationTimestamp { get; set; }

		[JsonPropertyName("origin")]
		public MessageOrigin Origin { get; set; }
	}

	public class MessageOrigin
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }
	}

	public class MessagePricing
	{
		[JsonPropertyName("pricing_model")]
		public string PricingModel { get; set; }

		[JsonPropertyName("billable")]
		public bool Billable { get; set; }

		[JsonPropertyName("category")]
		public string Category { get; set; }
	}

	public class FailedMessageError
	{
		[JsonPropertyName("code")]
		public long Code { get; set; }

        [JsonPropertyName("details")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Details { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

        [JsonPropertyName("error_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public MessageErrorData MessageErrorData { get; set; }

		[JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Message { get; set; }

        [JsonPropertyName("href")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Href { get; set; }
	}

	public class MessageErrorData
	{
		[JsonPropertyName("details")]
		public string Details { get; set; }
	}
}