using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Templates
{
    public class TemplateComponent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }  // "HEADER", "BODY", "FOOTER", "BUTTONS"

        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Text { get; set; }  // Used for header, body and footer. Should be null when Type is BUTTONS

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Format { get; set; }  // Used for header (TEXT)

        /// <summary>
        /// Use this property only when sending a template. Do not using when creating a template.
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object[] Parameters { get; set; }  // Used for body, buttons, and media headers

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("example")]
        public TemplateComponentParameterExample Example { get; set; }

        [JsonPropertyName("buttons")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<TemplateComponentButton> Buttons { get; set; }
    }

    public class TemplateComponentButton
    {
        /// <summary>
        /// Supported types: "QUICK_REPLY", "URL", "PHONE_NUMBER". Not supported yet: "COPY_CODE", "FLOW".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }  // Button text

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("url")]
        public string Url { get; set; }  // URL for URL button

        /// <summary>
        /// <para>Alphanumeric string. Business phone number to be called when the user taps the button.</para>
        /// <para>Note that some countries have special phone numbers that have leading zeros after the country calling code (e.g., +55-0-955-585-95436). If you assign one of these numbers to the button, the leading zero will be stripped from the number. If your number will not work without the leading zero, assign an alternate number to the button, or add the number as message body text.</para>
        /// <para>20 characters maximum.</para>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Required if Url contains a variable. The variable must be in the format {{1}}, {{2}}, etc.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("example")]
        public List<string> Example { get; set; }
    }

    public class TemplateComponentParameterExample
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("header_text")]
        public List<string> HeaderText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("header_text_named_params")]
        public List<TemplateComponentNamedParameter> HeaderTextNamedParameters { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("body_text")]
        public List<List<string>> BodyText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("body_text_named_params")]
        public List<TemplateComponentNamedParameter> BodyTextNamedParameters { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("footer_text")]
        public List<string> FooterText { get; set; }
    }

    public class TemplateComponentNamedParameter
    {
        /// <summary>
        /// Must be lowercase letters and underscores only.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("param_name")]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("example")]
        public string Example { get; set; }
    }
}
