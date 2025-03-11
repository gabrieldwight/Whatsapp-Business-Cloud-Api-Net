using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// When a message with media is received, the WhatsApp Business API downloads the media. A notification is sent to your Webhook once the media is downloaded.
    /// The Webhook notification contains information that identifies the media object and enables you to find and retrieve the object. Use the media endpoints to retrieve the media.
    /// </summary>
    public class DocumentMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<DocumentMessageEntry> Entry { get; set; }
    }

    public class DocumentMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<DocumentMessageChange> Changes { get; set; }
    }

    public class DocumentMessageChange
    {
        [JsonPropertyName("value")]
        public DocumentMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class DocumentMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public DocumentMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<DocumentMessage> Messages { get; set; }
    }

    public class DocumentMessage : GenericMessage
    {

        [JsonPropertyName("document")]
        public Document Document { get; set; }

        [JsonPropertyName("context")]
        public DocumentMessageContext? Context { get; set; }
    }

    public class Document
    {
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class DocumentMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class DocumentMessageContext
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
