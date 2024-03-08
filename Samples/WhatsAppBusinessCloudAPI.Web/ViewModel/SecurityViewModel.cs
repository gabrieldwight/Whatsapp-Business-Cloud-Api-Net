using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
	public class SecurityViewModel
	{
		public int RandomStringLength { get; set; }
		
		public string? RandomString { get; set; }
	}
}
