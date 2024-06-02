using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveTemplateMessageRequest
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
        public InteractiveMessageTemplate Template { get; set; }
    }

    public class InteractiveMessageTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public InteractiveMessageLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<InteractiveMessageComponent> Components { get; set; }
    }

    public class InteractiveMessageComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<InteractiveMessageParameter> Parameters { get; set; }

        [JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SubType { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }
    }

    public class InteractiveMessageParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageImage Image { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageCurrency Currency { get; set; }

        [JsonProperty("date_time", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageDateTime DateTime { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public string Payload { get; set; }

        [JsonProperty("document", NullValueHandling = NullValueHandling.Ignore)]
        public InteractiveMessageDocument Document { get; set; }
    }

    public class InteractiveMessageCurrency
    {
        [JsonProperty("fallback_value")]
        public string FallbackValue { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount_1000")]
        public long Amount1000 { get; set; }
    }

    public class InteractiveMessageDateTime
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

    public class InteractiveMessageImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    public class InteractiveMessageDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("filename")]
        public string FileName { get; set; }
    }

    public class InteractiveMessageLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}