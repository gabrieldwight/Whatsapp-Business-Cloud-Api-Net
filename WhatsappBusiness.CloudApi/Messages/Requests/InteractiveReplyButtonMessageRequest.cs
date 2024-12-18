using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
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
        public InteractiveReplyButtonMessage Interactive { get; set; }

		[JsonProperty("biz_opaque_callback_data", NullValueHandling = NullValueHandling.Ignore)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class InteractiveReplyButtonMessage
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "button";

		[JsonProperty("header")]
		public ReplyButtonHeader Header { get; set; }

		[JsonProperty("body")]
        public ReplyButtonBody Body { get; set; }

		[JsonProperty("footer")]
		public ReplyButtonFooter Footer { get; set; }

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

	public class ReplyButtonHeader
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
		public ReplyButtonImage ReplyButtonImage { get; set; }

		[JsonProperty("document", NullValueHandling = NullValueHandling.Ignore)]
		public ReplyButtonDocument ReplyButtonDocument { get; set; }

		[JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
		public ReplyButtonVideo ReplyButtonVideo { get; set; }
	}

	public class ReplyButtonFooter
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}

    public class ReplyButtonImage
    {
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaId { get; set; }

		[JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaUrl { get; set; }
    }

	public class ReplyButtonDocument
	{
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaId { get; set; }

		[JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaUrl { get; set; }
	}

	public class ReplyButtonVideo
	{
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaId { get; set; }

		[JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
		public string MediaUrl { get; set; }
	}
}