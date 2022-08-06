using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class TextMessageReplyRequest : TextMessageRequest
    {
        [JsonProperty("context")]
        public TextMessageContext Context { get; set; }
    }

    public class TextMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}