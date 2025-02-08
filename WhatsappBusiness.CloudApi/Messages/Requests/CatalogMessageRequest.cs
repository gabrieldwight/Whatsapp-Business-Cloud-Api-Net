using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CatalogMessageRequest
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
		public string Type { get; set; }

		[JsonPropertyName("interactive")]
		public CatalogMessageInteractive Interactive { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class CatalogMessageInteractive
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("body")]
		public CatalogMessageBody Body { get; set; }

		[JsonPropertyName("action")]
		public CatalogMessageAction Action { get; set; }

		[JsonPropertyName("footer")]
		public CatalogMessageFooter Footer { get; set; }
	}

	public class CatalogMessageAction
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("parameters")]
		public CatalogMessageParameters Parameters { get; set; }
	}

	public class CatalogMessageParameters
	{
		[JsonPropertyName("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }
	}

	public class CatalogMessageBody
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class CatalogMessageFooter : CatalogMessageBody
	{

	}
}
