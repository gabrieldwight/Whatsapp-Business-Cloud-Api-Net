using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class TextMessage : GenericMessage
    {
        [JsonPropertyName("text")]
        public TextMessageText Text { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class TextMessageText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }    

}