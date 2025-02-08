using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ImageMessageByUrlReplyRequest : ImageMessageByUrlRequest
    {
        [JsonPropertyName("context")]
        public ImageMessageUrlContext Context { get; set; }
    }

    public class ImageMessageUrlContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}