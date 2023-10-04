using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class LimitedTimeOfferTemplateMessageRequest
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
		public LimitedTimeOfferMessageTemplate Template { get; set; }
	}

	public class LimitedTimeOfferMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public LimitedTimeOfferMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<LimitedTimeOfferMessageComponent> Components { get; set; }
	}

	public class LimitedTimeOfferMessageComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<LimitedTimeOfferMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }
	}

	public class LimitedTimeOfferMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
		public LimitedTimeOfferMessageImage Image { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("limited_time_offer", NullValueHandling = NullValueHandling.Ignore)]
		public LimitedTimeOffer LimitedTimeOffer { get; set; }

		[JsonProperty("coupon_code", NullValueHandling = NullValueHandling.Ignore)]
		public string CouponCode { get; set; }
	}

	public class LimitedTimeOfferMessageImage
	{
		[JsonProperty("id")]
		public string Id { get; set; }
	}

	public class LimitedTimeOffer
	{
		[JsonProperty("expiration_time_ms")]
		public long ExpirationTimeMs { get; set; }
	}

	public class LimitedTimeOfferMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
