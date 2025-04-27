using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class AuthenticationTemplateMessageRequest
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
		public AuthenticationMessageTemplate Template { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

	public class AuthenticationMessageTemplate
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("language")]
		public AuthenticationMessageLanguage Language { get; set; }

		[JsonPropertyName("components")]
		public List<AuthenticationMessageComponent> Components { get; set; }
	}

	public class AuthenticationMessageComponent
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		/// <summary>
		/// Only to be used for template creation do not set this property when sending auth template messages
		/// </summary>
		[JsonPropertyName("add_security_recommendation")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool AddSecurityRecommendation { get; set; }

		/// <summary>
		/// Only to be used for template creation do not set this property when sending auth template messages
		/// </summary>
		[JsonPropertyName("code_expiration_minutes")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int CodeExpirationMinutes { get; set; }

		[JsonPropertyName("parameters")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<AuthenticationMessageParameter> Parameters { get; set; }

		[JsonPropertyName("sub_type")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string SubType { get; set; }

		[JsonPropertyName("index")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public long? Index { get; set; }

		[JsonIgnore]
		public bool IsTemplateCreation { get; set; }

		[JsonPropertyName("buttons")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("otp_type")]
		public string OtpType { get; set; }

		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Text { get; set; }

		[JsonPropertyName("autofill_text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string AutofillText { get; set; }

		[JsonPropertyName("zero_tap_terms_accepted")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ZeroTapTermsAccepted { get; set; }

		[JsonPropertyName("supported_apps")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<AuthenticationMessageSupportedApps> SupportedApps { get; set; }
	}

	public class AuthenticationMessageSupportedApps
	{
		[JsonPropertyName("package_name")]
		public string PackageName { get; set; }

		[JsonPropertyName("signature_hash")]
		public string SignatureHash { get; set; }
	}

	public class AuthenticationMessageParameter
	{
		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("parameter_name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParameterName { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
	}

	public class AuthenticationMessageLanguage
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }
	}
}