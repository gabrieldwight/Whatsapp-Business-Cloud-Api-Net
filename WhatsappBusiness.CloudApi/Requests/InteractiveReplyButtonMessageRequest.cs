using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Requests
{
    public class InteractiveReplyButtonMessageRequest
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
        public ReplyButtonInteractive Interactive { get; set; }
    }

    public class ReplyButtonInteractive
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("body")]
        public ReplyButtonBody Body { get; set; }

        [JsonProperty("action")]
        public ReplyButtonAction Action { get; set; }
    }

    public class ReplyButtonAction
    {
        [JsonProperty("buttons")]
        public List<ReplyButton> Buttons { get; set; }
    }

    public class ReplyButton
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reply")]
        public Reply Reply { get; set; }
    }

    public class Reply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class ReplyButtonBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}