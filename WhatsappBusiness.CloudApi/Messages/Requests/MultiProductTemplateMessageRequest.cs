using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class MultiProductTemplateMessageRequest
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
		public MPMTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class MPMTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public MPMLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<MPMComponent> Components { get; set; }
	}

	public class MPMComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		public List<MPMParameter> Parameters { get; set; }

		[JsonPropertyName("sub_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

		[JsonPropertyName("index")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Index { get; set; }
	}

	public class MPMParameter
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
		public MPMAction Action { get; set; }
	}

	public class MPMAction
	{
		[JsonPropertyName("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }

		[JsonPropertyName("sections")]
		public List<MPMSection> Sections { get; set; }
	}

	public class MPMSection
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("product_items")]
		public List<MPMProductItem> ProductItems { get; set; }
	}

	public class MPMProductItem
	{
		[JsonPropertyName("product_retailer_id")]
		public string ProductRetailerId { get; set; }
	}

	public class MPMLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}