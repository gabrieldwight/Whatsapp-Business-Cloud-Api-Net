namespace WhatsappBusiness.CloudApi.Interfaces;
public interface IMessage
{
    string From { get; set; }
    string Id { get; set; }
    string Timestamp { get; set; }
    string Type { get; set; }
}