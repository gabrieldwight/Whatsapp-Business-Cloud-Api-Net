using System.Collections.Generic;
using WhatsappBusiness.CloudApi.Webhook;

namespace WhatsappBusiness.CloudApi.Interfaces;
public interface IGenericStatus
{
    public string Id { get; set; }

    public string Type { get; set; } // To be used in WhatsApp Cloud API to get WhatsApp Call Status

    public string Status { get; set; }

    public string Timestamp { get; set; }

    public string RecipientId { get; set; }

	public string RecipientUserId { get; set; }

	public string ParentRecipientUserId { get; set; }

	public string RecipientType { get; set; }

    public string RecipientParticipantId { get; set; }

    public string RecipientIdentityKeyHash { get; set; }

	public string BizOpaqueCallbackData { get; set; }

	public Conversation Conversation { get; set; }

    public Pricing Pricing { get; set; }

    public List<MessageError> Errors { get; set; }
}