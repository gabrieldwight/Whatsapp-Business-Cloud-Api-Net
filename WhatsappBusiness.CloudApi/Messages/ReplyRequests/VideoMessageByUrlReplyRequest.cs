using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class VideoMessageByUrlReplyRequest : VideoMessageByUrlRequest
    {
        [JsonPropertyName("context")]
        public VideoMessageUrlContext Context { get; set; }
    }

    public class VideoMessageUrlContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}