using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class AudioMessageByUrlReplyRequest : AudioMessageByUrlRequest
    {
        [JsonProperty("context")]
        public AudioMessageUrlContext Context { get; set; }
    }

    public class AudioMessageUrlContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}