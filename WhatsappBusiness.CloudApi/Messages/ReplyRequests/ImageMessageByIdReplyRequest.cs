using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ImageMessageByIdReplyRequest : ImageMessageByIdRequest
    {
        [JsonPropertyName("context")]
        public ImageMessageContext Context { get; set; }
    }

    public class ImageMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}