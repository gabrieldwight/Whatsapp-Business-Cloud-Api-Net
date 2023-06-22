using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class MultiProductTemplateMessageRequest
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
		public MPMTemplate Template { get; set; }
	}

	public class MPMTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public MPMLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<MPMComponent> Components { get; set; }
	}

	public class MPMComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<MPMParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }
	}

	public class MPMParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
		public MPMAction Action { get; set; }
	}

	public class MPMAction
	{
		[JsonProperty("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }

		[JsonProperty("sections")]
		public List<MPMSection> Sections { get; set; }
	}

	public class MPMSection
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("product_items")]
		public List<MPMProductItem> ProductItems { get; set; }
	}

	public class MPMProductItem
	{
		[JsonProperty("product_retailer_id")]
		public string ProductRetailerId { get; set; }
	}

	public class MPMLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}