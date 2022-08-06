using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class VideoMessageByUrlReplyRequest : VideoMessageByUrlRequest
    {
        [JsonProperty("context")]
        public VideoMessageUrlContext Context { get; set; }
    }

    public class VideoMessageUrlContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}