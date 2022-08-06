using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class DocumentMessageByUrlReplyRequest : DocumentMessageByUrlRequest
    {
        [JsonProperty("context")]
        public DocumentMessageUrlContext Context { get; set; }
    }

    public class DocumentMessageUrlContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}