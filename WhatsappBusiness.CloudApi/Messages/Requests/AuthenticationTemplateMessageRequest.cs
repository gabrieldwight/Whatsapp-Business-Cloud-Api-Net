using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class AuthenticationTemplateMessageRequest
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
		public AuthenticationMessageTemplate Template { get; set; }
	}

	public class AuthenticationMessageTemplate
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("language")]
		public AuthenticationMessageLanguage Language { get; set; }

		[JsonProperty("components")]
		public List<AuthenticationMessageComponent> Components { get; set; }
	}

	public class AuthenticationMessageComponent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		/// <summary>
		/// Only to be used for template creation do not set this property when sending auth template messages
		/// </summary>
		[JsonProperty("add_security_recommendation", NullValueHandling = NullValueHandling.Ignore)]
		public bool AddSecurityRecommendation { get; set; }

		/// <summary>
		/// Only to be used for template creation do not set this property when sending auth template messages
		/// </summary>
		[JsonProperty("code_expiration_minutes", NullValueHandling = NullValueHandling.Ignore)]
		public int CodeExpirationMinutes { get; set; }

		[JsonProperty("parameters")]
		public List<AuthenticationMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }

		[JsonIgnore]
		public bool IsTemplateCreation { get; set; }

		public bool ShouldSerializeCodeExpirationMinutes()
		{
			// Only to be used for template creation do not set this property when sending auth template messages
			return CodeExpirationMinutes > 0;
		}

		public bool ShouldSerializeAddSecurityRecommendation()
		{
			// Only to be used for template creation do not set this property when sending auth template messages
			return (IsTemplateCreation) ? AddSecurityRecommendation : false;
		}
	}

	public class AuthenticationMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class AuthenticationMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}