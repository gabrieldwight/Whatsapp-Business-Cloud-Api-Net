using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When you receive a media message containing a sticker, WhatsApp Business API downloads the sticker and a notification is sent to your Webhook once the sticker is downloaded.
    /// The Webhook notification contains information that identifies the media object and allows you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    
    
    public class StickerMessage : GenericMessage
    {        

        [JsonPropertyName("sticker")]
        public Sticker Sticker { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class Sticker
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

		[JsonPropertyName("animated")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool Animated { get; set; }
	}
    
}