using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
	public class QRCodeMessageViewModel
	{
		public string Message { get; set; }
		public List<SelectListItem> ImageFormat { get; set; }
		public string SelectedImageFormat { get; set; }
	}
}
