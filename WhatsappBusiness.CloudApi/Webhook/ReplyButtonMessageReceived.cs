using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When your customer clicks on a quick reply button in an interactive message template, a response is sent.
    /// </summary>
    public class ReplyButtonMessage:GenericMessage
    {
        [JsonPropertyName("context")]
        public MessageContext Context { get; set; }
        

        [JsonPropertyName("interactive")]
        public ReplyButtonMessageInteractive Interactive { get; set; }
    }
    

    public class ReplyButtonMessageInteractive
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("button_reply")]
        public ButtonReply ButtonReply { get; set; }
    }

    public class ButtonReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

}