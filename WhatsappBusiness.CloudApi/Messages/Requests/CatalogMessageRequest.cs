using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CatalogMessageRequest
	{
		[JsonProperty("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonProperty("recipient_type")]
		public string RecipientType { get; private set; } = "individual";

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("interactive")]
		public CatalogMessageInteractive Interactive { get; set; }
	}

	public class CatalogMessageInteractive
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("body")]
		public CatalogMessageBody Body { get; set; }

		[JsonProperty("action")]
		public CatalogMessageAction Action { get; set; }

		[JsonProperty("footer")]
		public CatalogMessageFooter Footer { get; set; }
	}

	public class CatalogMessageAction
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("parameters")]
		public CatalogMessageParameters Parameters { get; set; }
	}

	public class CatalogMessageParameters
	{
		[JsonProperty("thumbnail_product_retailer_id")]
		public string ThumbnailProductRetailerId { get; set; }
	}

	public class CatalogMessageBody
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class CatalogMessageFooter : CatalogMessageBody
	{

	}
}
