using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class StickerMessageByIdReplyRequest : StickerMessageByIdRequest
    {
        [JsonPropertyName("context")]
        public StickerMessageContext Context { get; set; }
    }

    public class StickerMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}