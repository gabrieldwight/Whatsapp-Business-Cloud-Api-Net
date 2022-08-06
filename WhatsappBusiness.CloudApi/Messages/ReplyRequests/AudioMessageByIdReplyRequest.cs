using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class AudioMessageByIdReplyRequest : AudioMessageByIdRequest
    {

        [JsonProperty("context")]
        public AudioMessageContext Context { get; set; }
    }

    public class AudioMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}