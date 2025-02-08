using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class SingleProductTemplateMessageRequest
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
		public string Type { get; private set; } = "template";

		[JsonPropertyName("template")]
		public SPMTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class SPMTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public SPMLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<SPMComponent> Components { get; set; }
	}

	public class SPMComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		public List<SPMParameter> Parameters { get; set; }

		[JsonPropertyName("format")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Format { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("buttons")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<SPMButtons> Buttons { get; set; }
	}

	public class SPMParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("product")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SPMProduct Product { get; set; }
	}

	public class SPMProduct
	{
		[JsonPropertyName("product_retailer_id")]
		public string ProductId { get; set; }

		[JsonPropertyName("catalog_id")]
		public string CatalogId { get; set; }
	}

	public class SPMButtons
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class SPMLanguage
	{
		[JsonPropertyName("policy")]
		public string Policy { get; set; }

		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}
