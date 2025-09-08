using WhatsappBusiness.CloudApi.Configurations;

namespace WhatsappBusiness.CloudApi.Interfaces
{
	public interface IWhatsAppBusinessClientFactory
	{
		IWhatsAppBusinessClient Create(WhatsAppBusinessCloudApiConfig config);
	}
}
