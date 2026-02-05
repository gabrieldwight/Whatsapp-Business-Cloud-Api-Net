using WhatsappBusiness.CloudApi.Webhook;

namespace WhatsappBusiness.CloudApi.Interfaces;
public interface IGenericMessage
{
    string From { get; set; }
    string FromUserId { get; set; }
	string GroupId { get; set; }
	string Id { get; set; }
    string Timestamp { get; set; }
    string Type { get; set; }

    public MessageContext? Context { get; set; }
}