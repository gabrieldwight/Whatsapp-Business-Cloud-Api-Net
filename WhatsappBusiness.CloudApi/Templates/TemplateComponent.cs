namespace WhatsappBusiness.CloudApi.Templates
{
	public class TemplateComponent
	{
		public string Type { get; set; }  // "header", "body", "footer", "button"
		public string Text { get; set; }  // Used for header (text) and footer
		public object[] Parameters { get; set; }  // Used for body, buttons, and media headers
	}
}
