using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class LimitedTimeOfferTemplateMessageRequest
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
		public LimitedTimeOfferMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class LimitedTimeOfferMessageTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public LimitedTimeOfferMessageLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<LimitedTimeOfferMessageComponent> Components { get; set; }
	}

	public class LimitedTimeOfferMessageComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameters")]
		public List<LimitedTimeOfferMessageParameter> Parameters { get; set; }

		[JsonPropertyName("sub_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

		[JsonPropertyName("index")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Index { get; set; }
	}

	public class LimitedTimeOfferMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("image")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public LimitedTimeOfferMessageImage Image { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("limited_time_offer")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public LimitedTimeOffer LimitedTimeOffer { get; set; }

		[JsonPropertyName("coupon_code")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string CouponCode { get; set; }
	}

	public class LimitedTimeOfferMessageImage
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	public class LimitedTimeOffer
	{
		[JsonPropertyName("expiration_time_ms")]
		public long ExpirationTimeMs { get; set; }
	}

	public class LimitedTimeOfferMessageLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}
