using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class LocationMessageReplyRequest : LocationMessageRequest
    {
        [JsonPropertyName("context")]
        public LocationMessageContext Context { get; set; }
    }

    public class LocationMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}
