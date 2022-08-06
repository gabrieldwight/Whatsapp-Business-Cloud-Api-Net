using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ImageMessageByIdReplyRequest : ImageMessageByIdRequest
    {
        [JsonProperty("context")]
        public ImageMessageContext Context { get; set; }
    }

    public class ImageMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}