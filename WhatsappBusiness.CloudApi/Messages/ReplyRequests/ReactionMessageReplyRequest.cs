using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ReactionMessageReplyRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "reaction";

        [JsonProperty("reaction")]
        public Reaction Reaction { get; set; }
    }

    public class Reaction
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("emoji")]
        public string Emoji { get; set; }
    }
}
