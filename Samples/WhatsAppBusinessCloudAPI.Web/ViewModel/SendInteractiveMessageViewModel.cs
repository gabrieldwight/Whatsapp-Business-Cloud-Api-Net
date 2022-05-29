using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
    public class SendInteractiveMessageViewModel
    {
        public string RecipientPhoneNumber { get; set; }
        public List<SelectListItem> InteractiveType { get; set; }
        public string SelectedInteractiveType { get; set; }
        public string Message { get; set;}
    }
}
