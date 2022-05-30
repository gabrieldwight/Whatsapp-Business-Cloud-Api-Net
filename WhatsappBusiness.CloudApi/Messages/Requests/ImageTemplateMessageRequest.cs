using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class ImageTemplateMessageRequest
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
        public ImageMessageTemplate Template { get; set; }
    }

    public class ImageMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public ImageMessageLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<ImageMessageComponent> Components { get; set; }
    }

    public class ImageMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<ImageMessageParameter> Parameters { get; set; }
    }

    public class ImageMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public class Image
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class ImageMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}