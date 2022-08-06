using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ImageMessageByUrlReplyRequest : ImageMessageByUrlRequest
    {
        [JsonProperty("context")]
        public ImageMessageUrlContext Context { get; set; }
    }

    public class ImageMessageUrlContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}