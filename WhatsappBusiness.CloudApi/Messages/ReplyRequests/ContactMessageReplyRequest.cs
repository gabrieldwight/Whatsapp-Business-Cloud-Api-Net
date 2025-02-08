using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ContactMessageReplyRequest : ContactMessageRequest
    {
        [JsonPropertyName("context")]
        public ContactMessageContext Context { get; set; }
    }

    public class ContactMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}