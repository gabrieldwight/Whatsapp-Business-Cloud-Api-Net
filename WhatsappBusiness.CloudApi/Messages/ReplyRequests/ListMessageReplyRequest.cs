using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ListMessageReplyRequest : InteractiveListMessageRequest
    {
        [JsonPropertyName("context")]
        public ListMessageContext Context { get; set; }
    }

    public class ListMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}
