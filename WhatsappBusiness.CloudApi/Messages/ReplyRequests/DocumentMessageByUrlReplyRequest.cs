using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class DocumentMessageByUrlReplyRequest : DocumentMessageByUrlRequest
    {
        [JsonPropertyName("context")]
        public DocumentMessageUrlContext Context { get; set; }
    }

    public class DocumentMessageUrlContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}