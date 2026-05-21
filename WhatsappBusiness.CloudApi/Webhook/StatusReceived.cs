using System.Collections.Generic;
using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Interfaces;
namespace WhatsappBusiness.CloudApi.Webhook;




public class StatusReceived<TStatusType> where TStatusType : IGenericStatus
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("entry")]
    public List<StatusEntry<TStatusType>> Entry { get; set; }

}

public class StatusEntry<TStatusType> where TStatusType : IGenericStatus
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("changes")]
    public List<StatusChange<TStatusType>> Changes { get; set; }
}

public class StatusChange<TStatusType> where TStatusType : IGenericStatus
{
    [JsonPropertyName("value")]
    public StatusValue<TStatusType> Value { get; set; }

    [JsonPropertyName("field")]
    public string Field { get; set; }
}

public class StatusValue<TStatusType> where TStatusType : IGenericStatus
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; }

    [JsonPropertyName("metadata")]
    public MessageMetadata Metadata { get; set; }

    [JsonPropertyName("contacts")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<Contact> Contacts { get; set; }

	[JsonPropertyName("statuses")]
    public List<TStatusType> Statuses { get; set; }
}


public class GenericStatus : IGenericStatus
{
    [JsonPropertyName("id")]
    public virtual string Id { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string Type { get; set; }

	[JsonPropertyName("status")]
    public virtual string Status { get; set; }

    [JsonPropertyName("timestamp")]
    public virtual string Timestamp { get; set; }

    [JsonPropertyName("recipient_id")]
    public virtual string RecipientId { get; set; }

    [JsonPropertyName("recipient_user_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string RecipientUserId { get; set; }

	[JsonPropertyName("parent_recipient_user_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string ParentRecipientUserId { get; set; }

	[JsonPropertyName("recipient_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string RecipientType { get; set; }

	[JsonPropertyName("recipient_participant_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string RecipientParticipantId { get; set; }

	[JsonPropertyName("recipient_identity_key_hash")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string RecipientIdentityKeyHash { get; set; }

	[JsonPropertyName("biz_opaque_callback_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual string BizOpaqueCallbackData { get; set; }

	[JsonPropertyName("conversation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual Conversation Conversation { get; set; }

    [JsonPropertyName("pricing")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual Pricing Pricing { get; set; }

	[JsonPropertyName("errors")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public virtual List<MessageError> Errors { get; set; }
}


public class Pricing
{
    [JsonPropertyName("pricing_model")]
    public string PricingModel { get; set; }

    [JsonPropertyName("billable")]
    public bool Billable { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }
}

public class Conversation
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("expiration_timestamp")]
    public string ExpirationTimestamp { get; set; }

    [JsonPropertyName("origin")]
    public Origin? Origin { get; set; }

    [JsonPropertyName("type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Type { get; set; }
}

public class Origin
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class MessageError
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
	public ErrorData MessageErrorData { get; set; }

	[JsonPropertyName("message")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Message { get; set; }

	[JsonPropertyName("href")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Href { get; set; }
}

public class ErrorData
{
	[JsonPropertyName("details")]
	public string Details { get; set; }
}