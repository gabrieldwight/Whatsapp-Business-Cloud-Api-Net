using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class DocumentMessageByIdReplyRequest : DocumentMessageByIdRequest
    {
        [JsonPropertyName("context")]
        public DocumentMessageContext Context { get; set; }
    }

    public class DocumentMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}