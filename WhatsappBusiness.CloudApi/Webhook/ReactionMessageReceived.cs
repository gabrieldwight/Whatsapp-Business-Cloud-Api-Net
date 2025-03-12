using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A reaction message you received from a customer
    /// </summary>      

    public class ReactionMessage:GenericMessage
    {
        [JsonPropertyName("reaction")]
        public ReactionMessageText Reaction { get; set; }
    }

    public class ReactionMessageText
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; }
    }

    
}