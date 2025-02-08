using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class TextMessageReplyRequest : TextMessageRequest
    {
        [JsonPropertyName("context")]
        public TextMessageContext Context { get; set; }
    }

    public class TextMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}