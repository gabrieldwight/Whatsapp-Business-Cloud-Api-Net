using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A message you received from a customer that is not supported.
    /// </summary>
    public class UnknownMessageReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<UnknownMessageEntry> Entry { get; set; }
    }

    public class UnknownMessageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<UnknownMessageChange> Changes { get; set; }
    }

    public class UnknownMessageChange
    {
        [JsonPropertyName("value")]
        public UnknownMessageValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class UnknownMessageValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public UnknownMessageMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<UnknownMessage> Messages { get; set; }
    }



    public class UnknownMessage : GenericMessage
    {


        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }

    }

    public class Error
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class UnknownMessageMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}