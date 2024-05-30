using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class LocationTemplateMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "template";

        [JsonProperty("template")]
        public LocationMessageTemplate Template { get; set; }
    }

    public class LocationMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public LocationMessageLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<LocationMessageComponent> Components { get; set; }
    }

    public class LocationMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<LocationMessageParameter> Parameters { get; set; }
    }

    public class LocationMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public LocationDetails Location { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public class LocationDetails
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

    public class LocationMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
