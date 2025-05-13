using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A static location message you received from a customer.
    /// </summary>
    public class LocationMessage:GenericMessage
    {        

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

}