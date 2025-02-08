using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.ReplyRequests
{
    public class ReactionMessageReplyRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; private set; } = "reaction";

        [JsonPropertyName("reaction")]
        public Reaction Reaction { get; set; }
    }

    public class Reaction
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; }
    }
}
