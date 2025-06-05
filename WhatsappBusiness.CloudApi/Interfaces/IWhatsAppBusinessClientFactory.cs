using WhatsappBusiness.CloudApi.Configurations;

namespace WhatsappBusiness.CloudApi.Interfaces
{
	public interface IWhatsAppBusinessClientFactory
	{
		WhatsAppBusinessClient Create(WhatsAppBusinessCloudApiConfig config);
	}
}
