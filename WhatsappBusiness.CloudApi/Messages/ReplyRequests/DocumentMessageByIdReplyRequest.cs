using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class DocumentMessageByIdReplyRequest : DocumentMessageByIdRequest
    {
        [JsonProperty("context")]
        public DocumentMessageContext Context { get; set; }
    }

    public class DocumentMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}