using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class VideoMessageByIdReplyRequest : VideoMessageByIdRequest
    {
        [JsonPropertyName("context")]
        public VideoMessageContext Context { get; set; }
    }

    public class VideoMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}