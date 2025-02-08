using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class AudioMessageByIdReplyRequest : AudioMessageByIdRequest
    {
        [JsonPropertyName("context")]
        public AudioMessageContext Context { get; set; }
    }

    public class AudioMessageContext
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}