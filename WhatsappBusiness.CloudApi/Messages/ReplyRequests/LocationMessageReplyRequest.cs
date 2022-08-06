using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class LocationMessageReplyRequest : LocationMessageRequest
    {
        [JsonProperty("context")]
        public LocationMessageContext Context { get; set; }
    }

    public class LocationMessageContext
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}
