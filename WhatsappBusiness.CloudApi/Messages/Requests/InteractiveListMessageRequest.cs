using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveListMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; private set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "interactive";

        [JsonProperty("interactive")]
        public InteractiveListMessage Interactive { get; set; }
    }

    public class InteractiveListMessage
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "list";

        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("body")]
        public ListBody Body { get; set; }

        [JsonProperty("footer")]
        public Footer Footer { get; set; }

        [JsonProperty("action")]
        public ListAction Action { get; set; }
    }

    public class ListAction
    {
        [JsonProperty("button")]
        public string Button { get; set; }

        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class ListBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Header
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Footer
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}