namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
    public class SendTemplateMessageViewModel
    { //
        public string RecipientPhoneNumber { get; set; }
		public string Message { get; set; }         // This may be ignored when sending a Template
		public string TemplateName { get; set; }
        public string? MediaId { get; set; }
        public string? LinkUrl { get; set; }
        public string? TemplateParams { get; set; }

    }
}
