using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveCTAButtonMessageRequest
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
        public InteractiveCTAButtonMessage Interactive { get; set; }
    }

    public class InteractiveCTAButtonMessage
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "cta_url";

        [JsonProperty("header")]
        public CTAButtonHeader Header { get; set; }

        [JsonProperty("body")]
        public CTAButtonBody Body { get; set; }

        [JsonProperty("footer")]
        public CTAButtonFooter Footer { get; set; }

        [JsonProperty("action")]
        public CTAButtonAction Action { get; set; }
    }

    public class CTAButtonAction
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parameters")]
        public CTAButtonParameters Parameters { get; set; }
    }

    public class CTAButtonParameters
    {
        [JsonProperty("display_text")]
        public string DisplayText { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class CTAButtonHeader
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class CTAButtonBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class CTAButtonFooter
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
