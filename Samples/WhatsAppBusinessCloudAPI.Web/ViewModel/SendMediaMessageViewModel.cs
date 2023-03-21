using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
    public class SendMediaMessageViewModel
    {
        public string RecipientPhoneNumber { get; set; }
        public List<SelectListItem> MediaType { get; set; }
        public string SelectedMediaType { get; set; }
        public string Message { get; set; }
        public string? MediaLink { get; set; }
        public string? MediaId { get; set; }
    }
}
