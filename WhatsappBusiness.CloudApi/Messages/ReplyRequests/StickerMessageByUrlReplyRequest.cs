using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class StickerMessageByUrlReplyRequest : StickerMessageByUrlRequest
    {
        [JsonPropertyName("context")]
        public StickerMessageUrlContext Context { get; set; }
    }

    public class StickerMessageUrlContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}