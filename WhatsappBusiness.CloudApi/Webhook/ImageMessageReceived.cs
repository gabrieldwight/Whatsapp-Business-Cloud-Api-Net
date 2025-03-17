using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    
    

    public class ImageMessage: GenericMessage
    {    

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    
}