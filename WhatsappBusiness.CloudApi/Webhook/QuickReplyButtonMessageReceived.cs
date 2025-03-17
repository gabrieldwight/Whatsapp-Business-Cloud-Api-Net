using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    
    public class QuickReplyButtonMessage : GenericMessage
    {
        [JsonPropertyName("context")]
        public MessageContext Context { get; set; }
    

        [JsonPropertyName("button")]
        public QuickReplyButtonMessageButton Button { get; set; }
    }

    public class QuickReplyButtonMessageButton
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }

}