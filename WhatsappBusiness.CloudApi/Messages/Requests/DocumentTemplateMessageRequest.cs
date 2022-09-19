using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class DocumentTemplateMessageRequest
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
        public DocumentMessageTemplate Template { get; set; }
    }

    public class DocumentMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public DocumentMessageLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<DocumentMessageComponent> Components { get; set; }
    }

    public class DocumentMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<DocumentMessageParameter> Parameters { get; set; }
    }

    public class DocumentMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("document", NullValueHandling = NullValueHandling.Ignore)]
        public Document Document { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentTemplateCurrency Currency { get; set; }

        [JsonProperty("date_time", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentTemplateDateTime DateTime { get; set; }
    }

    public class DocumentTemplateCurrency
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount_1000")]
        public long Amount1000 { get; set; }
    }

    public class DocumentTemplateDateTime
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

    public class Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }
    }

    public class DocumentMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
