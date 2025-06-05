using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Interfaces;

namespace WhatsappBusiness.CloudApi
{
	public class WhatsAppBusinessClientFactory : IWhatsAppBusinessClientFactory
	{
		public WhatsAppBusinessClient Create(WhatsAppBusinessCloudApiConfig config)
		{
			return new WhatsAppBusinessClient(config);
		}
	}
}
