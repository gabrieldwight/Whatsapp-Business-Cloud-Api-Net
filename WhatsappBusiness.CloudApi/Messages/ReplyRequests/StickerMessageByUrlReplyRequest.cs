using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class StickerMessageByUrlReplyRequest : StickerMessageByUrlRequest
    {
        [JsonProperty("context")]
        public StickerMessageUrlContext Context { get; set; }
    }

    public class StickerMessageUrlContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}