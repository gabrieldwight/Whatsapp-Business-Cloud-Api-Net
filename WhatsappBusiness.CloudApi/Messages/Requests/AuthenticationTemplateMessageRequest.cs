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

		[JsonProperty("biz_opaque_callback_data", NullValueHandling = NullValueHandling.Ignore)]
		public string BizOpaqueCallbackData { get; set; }
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

		[JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
		public List<AuthenticationMessageParameter> Parameters { get; set; }

		[JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
		public string SubType { get; set; }

		[JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
		public long? Index { get; set; }

		[JsonIgnore]
		public bool IsTemplateCreation { get; set; }

		[JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
		public List<AuthenticationMessageButton> Buttons { get; set; }

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

	public class AuthenticationMessageButton
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("otp_type")]
		public string OtpType { get; set; }

		[JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		[JsonProperty("autofill_text", NullValueHandling = NullValueHandling.Ignore)]
		public string AutofillText { get; set; }

		[JsonProperty("zero_tap_terms_accepted", NullValueHandling = NullValueHandling.Ignore)]
		public string ZeroTapTermsAccepted { get; set; }

		[JsonProperty("supported_apps", NullValueHandling = NullValueHandling.Ignore)]
		public List<AuthenticationMessageSupportedApps> SupportedApps { get; set; }
	}

	public class AuthenticationMessageSupportedApps
	{
		[JsonProperty("package_name")]
		public string PackageName { get; set; }

		[JsonProperty("signature_hash")]
		public string SignatureHash { get; set; }
	}

	public class AuthenticationMessageParameter
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("parameter_name", NullValueHandling = NullValueHandling.Ignore)]
		public string ParameterName { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}

	public class AuthenticationMessageLanguage
	{
		[JsonProperty("code")]
		public string Code { get; set; }
	}
}