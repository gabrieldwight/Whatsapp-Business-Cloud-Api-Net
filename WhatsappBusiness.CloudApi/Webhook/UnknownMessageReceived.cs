using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A message you received from a customer that is not supported.
    /// </summary>
    public class UnknownMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<UnknownMessageEntry> Entry { get; set; }
    }

    public class UnknownMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<UnknownMessageChange> Changes { get; set; }
    }

    public class UnknownMessageChange
    {
        [JsonProperty("value")]
        public UnknownMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class UnknownMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public UnknownMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<UnknownMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<UnknownMessage> Messages { get; set; }
    }

    public class UnknownMessageContact
    {
        [JsonProperty("profile")]
        public UnknownMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class UnknownMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class UnknownMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class UnknownMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}