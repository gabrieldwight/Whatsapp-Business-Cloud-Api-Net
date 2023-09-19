using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CarouselTemplateMessageRequest
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
		public CarouselMessageTemplate Template { get; set; }
	}

	public class CarouselMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public CarouselMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<CarouselMessageTemplateComponent> Components { get; set; }
	}

	public class CarouselMessageTemplateComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
		public List<CarouselMessageParameter> Parameters { get; set; }

		[JsonProperty("cards", NullValueHandling = NullValueHandling.Ignore)]
		public List<CarouselMessageCard> Cards { get; set; }
	}

	public class CarouselMessageCard
	{
		[JsonProperty("card_index")]
		public int CardIndex { get; set; }

		[JsonProperty("components")]
		public List<CarouselCardComponent> Components { get; set; }
	}

	public class CarouselCardComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<CardMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }
	}

	public class CardMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
		public CardImage Image { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
		public string Payload { get; set; }
	}

	public class CardImage
	{
		[JsonProperty("id")]
		public string Id { get; set; }
	}

	public partial class CarouselMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class CarouselMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
