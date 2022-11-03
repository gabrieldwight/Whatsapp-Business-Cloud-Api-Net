using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class DocumentMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<DocumentMessageEntry> Entry { get; set; }
    }

    public class DocumentMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<DocumentMessageChange> Changes { get; set; }
    }

    public class DocumentMessageChange
    {
        [JsonProperty("value")]
        public DocumentMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class DocumentMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public DocumentMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<DocumentMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<DocumentMessage> Messages { get; set; }
    }

    public class DocumentMessageContact
    {
        [JsonProperty("profile")]
        public DocumentMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class DocumentMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class DocumentMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("document")]
        public Document Document { get; set; }

        [JsonProperty("context")]
        public DocumentMessageContext? Context { get; set; }
    }

    public class Document
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class DocumentMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class DocumentMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
