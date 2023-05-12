using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
	public class UploadMediaViewModel
	{
		public List<SelectListItem> UploadType { get; set; }
		public string SelectedUploadType { get; set; }
	}
}
