using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A static location message you received from a customer.
    /// </summary>
    public class StaticLocationMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<LocationMessageEntry> Entry { get; set; }
    }

    public class LocationMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<LocationMessageChange> Changes { get; set; }
    }

    public class LocationMessageChange
    {
        [JsonProperty("value")]
        public LocationMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class LocationMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public LocationMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<LocationMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<LocationMessage> Messages { get; set; }
    }

    public class LocationMessageContact
    {
        [JsonProperty("profile")]
        public LocationMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class LocationMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class LocationMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("context")]
        public LocationMessageContext? Context { get; set; }
    }

    public class Location
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class LocationMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class LocationMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}