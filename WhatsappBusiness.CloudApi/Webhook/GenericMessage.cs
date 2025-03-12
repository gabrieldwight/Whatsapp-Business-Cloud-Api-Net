using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Webhook;

public abstract class GenericMessage : IGenericMessage
{
    [JsonPropertyName("from")]
    public virtual string From { get; set; }

    [JsonPropertyName("id")]
    public virtual string Id { get; set; }

    [JsonPropertyName("timestamp")]
    public virtual string Timestamp { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("context")]
    public MessageContext? Context { get; set; }

}