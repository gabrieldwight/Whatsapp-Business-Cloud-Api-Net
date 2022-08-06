using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ContactMessageReplyRequest : ContactMessageRequest
    {
        [JsonProperty("context")]
        public ContactMessageContext Context { get; set; }
    }

    public class ContactMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}