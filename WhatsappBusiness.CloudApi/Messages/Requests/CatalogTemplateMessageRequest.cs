using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CatalogTemplateMessageRequest
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
		public CatalogMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class CatalogMessageTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public CatalogMessageLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<CatalogMessageComponent> Components { get; set; }
	}

	public class CatalogMessageComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		public List<CatalogTemplateMessageParameter> Parameters { get; set; }

		[JsonPropertyName("sub_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

		[JsonPropertyName("index")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Index { get; set; }
	}

	public class CatalogTemplateMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("action")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CatalogTemplateMessageAction Action { get; set; }
	}

	public class CatalogTemplateMessageAction
	{
		[JsonPropertyName("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }
	}

	public class CatalogMessageLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}
