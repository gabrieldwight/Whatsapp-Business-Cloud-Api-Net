using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class WhatsAppGroupInviteLinkResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("invite_link")]
		public string InviteLink { get; set; }
	}
}
