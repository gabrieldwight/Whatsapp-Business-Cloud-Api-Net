using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class QRCodeMessageRequest
	{
		[JsonProperty("prefilled_message")]
		public string PrefilledMessage { get; set; }

		[JsonProperty("generate_qr_image", NullValueHandling = NullValueHandling.Ignore)]
		public string GenerateQRImage { get; set; }

		[JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
		public string Code { get; set; }
	}
}
