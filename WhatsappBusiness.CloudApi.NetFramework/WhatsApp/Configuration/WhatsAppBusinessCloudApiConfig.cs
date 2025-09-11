namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Configuration
{
    /// <summary>
    /// Configuration class for WhatsApp Business Cloud API
    /// </summary>
    public class WhatsAppBusinessCloudApiConfig
    {
        /// <summary>
        /// WhatsApp Business ID
        /// </summary>
        public string WhatsAppBusinessId { get; set; }

        /// <summary>
        /// WhatsApp Access Token
        /// </summary>
        public string WhatsAppAccessToken { get; set; }

        /// <summary>
        /// WhatsApp Graph API Version (e.g., "v19.0")
        /// </summary>
        public string WhatsAppGraphApiVersion { get; set; } = "v23.0";

        /// <summary>
        /// WhatsApp Embedded Signup Meta App ID
        /// </summary>
        public string WhatsAppEmbeddedSignupMetaAppId { get; set; }

        /// <summary>
        /// WhatsApp Embedded Signup Meta App Secret
        /// </summary>
        public string WhatsAppEmbeddedSignupMetaAppSecret { get; set; }

        /// <summary>
        /// WhatsApp Embedded Signup Meta Configuration ID
        /// </summary>
        public string WhatsAppEmbeddedSignupMetaConfigurationId { get; set; }

        /// <summary>
        /// WhatsApp Embedded Signup Partner Solution ID
        /// </summary>
        public string WhatsAppEmbeddedSignupPartnerSolutionId { get; set; }
    }
}