using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CarouselTemplateMessageRequest
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
		public CarouselMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class CarouselMessageTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public CarouselMessageLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<CarouselMessageTemplateComponent> Components { get; set; }
	}

	public class CarouselMessageTemplateComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<CarouselMessageParameter> Parameters { get; set; }

		[JsonPropertyName("cards")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<CarouselMessageCard> Cards { get; set; }
	}

	public class CarouselMessageCard
	{
		[JsonPropertyName("card_index")]
		public int CardIndex { get; set; }

		[JsonPropertyName("components")]
		public List<CarouselCardComponent> Components { get; set; }
	}

	public class CarouselCardComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("format")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Format { get; set; }

		[JsonPropertyName("parameters")]
		public List<CardMessageParameter> Parameters { get; set; }

		[JsonPropertyName("sub_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

		[JsonPropertyName("index")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Index { get; set; }

		[JsonPropertyName("buttons")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<CardMessageButton> Buttons { get; set; }
	}

	public class CardMessageButton
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class CardMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("image")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CardImage Image { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("payload")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Payload { get; set; }
	}

	public class CardImage
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	public partial class CarouselMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class CarouselMessageLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}
