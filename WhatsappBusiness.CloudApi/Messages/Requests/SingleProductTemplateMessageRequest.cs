using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class SingleProductTemplateMessageRequest
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
		public SPMTemplate Template { get; set; }

		[JsonProperty("biz_opaque_callback_data", NullValueHandling = NullValueHandling.Ignore)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class SPMTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public SPMLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<SPMComponent> Components { get; set; }
	}

	public class SPMComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<SPMParameter> Parameters { get; set; }

		[JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
		public string Format { get; set; }

		[JsonProperty("text",  NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
		public List<SPMButtons> Buttons { get; set; }
	}

	public class SPMParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameter_name", NullValueHandling = NullValueHandling.Ignore)]
		public string ParameterName { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
		public SPMProduct Product { get; set; }
	}

	public class SPMProduct
	{
		[JsonProperty("product_retailer_id")]
		public string ProductId { get; set; }

		[JsonProperty("catalog_id")]
		public string CatalogId { get; set; }
	}

	public class SPMButtons
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class SPMLanguage
	{
		[JsonProperty("policy")]
		public string Policy { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
