using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class TextTemplateMessageRequest
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
        public TextMessageTemplate Template { get; set; }
    }

    public class TextMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public TextMessageLanguage Language { get; set; }

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

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateImage Image { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateCurrency Currency { get; set; }

        [JsonProperty("date_time", NullValueHandling = NullValueHandling.Ignore)]
        public TemplateDateTime DateTime { get; set; }
    }

    public class TemplateCurrency
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount_1000")]
        public long Amount1000 { get; set; }
    }

    public class TemplateDateTime
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("day_of_week")]
        public long DayOfWeek { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("month")]
        public long Month { get; set; }

        [JsonProperty("day_of_month")]
        public long DayOfMonth { get; set; }

        [JsonProperty("hour")]
        public long Hour { get; set; }

        [JsonProperty("minute")]
        public long Minute { get; set; }

        [JsonProperty("calendar")]
        public string Calendar { get; set; }
    }

    public class TemplateImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    public class TextMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
