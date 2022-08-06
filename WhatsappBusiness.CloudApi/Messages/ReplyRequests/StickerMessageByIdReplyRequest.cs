using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class StickerMessageByIdReplyRequest : StickerMessageByIdRequest
    {
        [JsonProperty("context")]
        public StickerMessageContext Context { get; set; }
    }

    public class StickerMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}