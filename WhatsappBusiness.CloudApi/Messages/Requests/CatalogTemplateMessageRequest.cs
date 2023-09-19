using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CatalogTemplateMessageRequest
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
		public CatalogMessageTemplate Template { get; set; }
	}

	public class CatalogMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public CatalogMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<CatalogMessageComponent> Components { get; set; }
	}

	public class CatalogMessageComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<CatalogTemplateMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }
	}

	public class CatalogTemplateMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
		public CatalogTemplateMessageAction Action { get; set; }
	}

	public class CatalogTemplateMessageAction
	{
		[JsonProperty("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }
	}

	public class CatalogMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
