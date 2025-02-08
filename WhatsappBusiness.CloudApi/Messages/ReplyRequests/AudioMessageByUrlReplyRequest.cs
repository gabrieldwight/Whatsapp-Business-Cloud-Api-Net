using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class AudioMessageByUrlReplyRequest : AudioMessageByUrlRequest
    {
        [JsonPropertyName("context")]
        public AudioMessageUrlContext Context { get; set; }
    }

    public class AudioMessageUrlContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}