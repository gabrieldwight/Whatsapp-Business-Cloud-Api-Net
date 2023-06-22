namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
    public class SendTemplateMessageViewModel
    {
        public string RecipientPhoneNumber { get; set; }
        public string TemplateName { get; set; }
        public string? MediaId { get; set; }
        public string? LinkUrl { get; set; }
    }
}
