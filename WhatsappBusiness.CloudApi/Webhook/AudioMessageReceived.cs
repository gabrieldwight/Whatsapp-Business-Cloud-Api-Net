using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class AudioMessage : GenericMessage
    {        

        [JsonPropertyName("audio")]
        public Audio Audio { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    
}
