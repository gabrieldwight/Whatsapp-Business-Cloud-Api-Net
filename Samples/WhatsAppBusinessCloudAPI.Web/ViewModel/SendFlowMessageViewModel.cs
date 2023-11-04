using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
	public class SendFlowMessageViewModel
	{
		public string RecipientPhoneNumber { get; set; }
		public List<SelectListItem> FlowAction { get; set; }
		public string SelectedFlowAction { get; set; }
		public List<SelectListItem> Mode { get; set; }
		public string SelectedMode { get; set; }
		public string ScreenId { get; set; }
		public string FlowToken { get; set; }
		public string FlowId { get; set; }
		public string FlowButtonText { get; set; }
	}
}
