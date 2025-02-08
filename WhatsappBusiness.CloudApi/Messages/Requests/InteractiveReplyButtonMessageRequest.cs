using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class InteractiveReplyButtonMessageRequest
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
        public InteractiveReplyButtonMessage Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveReplyButtonMessage
    {
        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "button";

		[JsonPropertyName("header")]
		public ReplyButtonHeader Header { get; set; }

		[JsonPropertyName("body")]
        public ReplyButtonBody Body { get; set; }

		[JsonPropertyName("footer")]
		public ReplyButtonFooter Footer { get; set; }

		[JsonPropertyName("action")]
        public ReplyButtonAction Action { get; set; }
    }

    public class ReplyButtonAction
    {
        [JsonPropertyName("buttons")]
        public List<ReplyButton> Buttons { get; set; }
    }

    public class ReplyButton
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("reply")]
        public Reply Reply { get; set; }
    }

    public class Reply
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public partial class ReplyButtonBody
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

	public class ReplyButtonHeader
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("image")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ReplyButtonImage ReplyButtonImage { get; set; }

		[JsonPropertyName("document")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ReplyButtonDocument ReplyButtonDocument { get; set; }

		[JsonPropertyName("video")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ReplyButtonVideo ReplyButtonVideo { get; set; }
	}

	public class ReplyButtonFooter
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

    public class ReplyButtonImage
    {
		[JsonPropertyName("id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaId { get; set; }

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaUrl { get; set; }
    }

	public class ReplyButtonDocument
	{
		[JsonPropertyName("id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaId { get; set; }

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaUrl { get; set; }
	}

	public class ReplyButtonVideo
	{
		[JsonPropertyName("id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaId { get; set; }

		[JsonPropertyName("link")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MediaUrl { get; set; }
	}
}