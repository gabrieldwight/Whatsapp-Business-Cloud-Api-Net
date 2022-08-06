using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class VideoMessageByIdReplyRequest : VideoMessageByIdRequest
    {
        [JsonProperty("context")]
        public VideoMessageContext Context { get; set; }
    }

    public class VideoMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}