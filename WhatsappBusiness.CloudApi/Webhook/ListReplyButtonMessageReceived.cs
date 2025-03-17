using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{    

    public class ListReplyButtonMessage : GenericMessage
    {
        [JsonPropertyName("context")]
        public MessageContext Context { get; set; }

        [JsonPropertyName("interactive")]
        public Interactive Interactive { get; set; }
    }
    

    public partial class Interactive
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("list_reply")]
        public ListReply ListReply { get; set; }
    }

    public partial class ListReply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
    
}