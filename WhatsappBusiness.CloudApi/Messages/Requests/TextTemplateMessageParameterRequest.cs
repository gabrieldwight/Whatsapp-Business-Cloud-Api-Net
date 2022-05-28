using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class TextTemplateMessageParameterRequest
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
        public TextMessageParameterTemplate Template { get; set; }
    }

    public class TextMessageParameterTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public TextMessageParameterLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<TextMessageComponent> Components { get; set; }
    }

    public class TextMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<TextMessageParameter> Parameters { get; set; }
    }

    public class TextMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public class TextMessageParameterLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}