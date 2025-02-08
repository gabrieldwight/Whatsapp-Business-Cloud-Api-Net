using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveListMessageRequest
    {
        [JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("recipient_type")]
		[JsonInclude]
		public string RecipientType { get; private set; } = "individual";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "interactive";

        [JsonPropertyName("interactive")]
        public InteractiveListMessage Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveListMessage
    {
        [JsonPropertyName("type")]
        [JsonInclude]
        public string Type { get; private set; } = "list";

        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("body")]
        public ListBody Body { get; set; }

        [JsonPropertyName("footer")]
        public Footer Footer { get; set; }

        [JsonPropertyName("action")]
        public ListAction Action { get; set; }
    }

    public class ListAction
    {
        [JsonPropertyName("button")]
        public string Button { get; set; }

        [JsonPropertyName("sections")]
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("rows")]
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class ListBody
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Header
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Footer
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}