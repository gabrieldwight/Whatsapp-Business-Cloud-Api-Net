using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class AudioMessage : GenericMessage
    {        
        [JsonPropertyName("audio")]
        public Audio Audio { get; set; }

        [JsonPropertyName("context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public MessageContext? Context { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

		[JsonPropertyName("sha256")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Sha256 { get; set; }

		[JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("voice")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Voice { get; set; }
	}
    
}
