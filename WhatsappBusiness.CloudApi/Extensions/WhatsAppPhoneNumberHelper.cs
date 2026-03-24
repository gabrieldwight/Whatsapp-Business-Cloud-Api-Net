namespace WhatsappBusiness.CloudApi.Extensions
{
	public static class WhatsAppPhoneNumberHelper
	{
		public static bool IsBsuid(string recipient)
		{
			return !recipient.StartsWith("+") && recipient.Contains(".");
		}
	}
}
