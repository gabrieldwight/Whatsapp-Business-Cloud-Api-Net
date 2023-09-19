using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class CouponCodeTemplateMessageRequest
	{
		[JsonProperty("messaging_product")]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("type")]
		public string Type { get; private set; } = "template";

		[JsonProperty("template")]
		public CouponCodeMessageTemplate Template { get; set; }
	}

	public class CouponCodeMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public CouponCodeMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<CouponCodeMessageComponent> Components { get; set; }
	}

	public class CouponCodeMessageComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameters")]
		public List<CouponCodeMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public int? Index { get; set; }
	}

	public class CouponCodeMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("coupon_code", NullValueHandling = NullValueHandling.Ignore)]
		public string CouponCode { get; set; }
	}

	public class CouponCodeMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
