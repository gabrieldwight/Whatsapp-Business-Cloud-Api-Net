using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
	public class QRCodeMessageRequest
	{
		[JsonPropertyName("prefilled_message")]
		public string PrefilledMessage { get; set; }

		[JsonPropertyName("generate_qr_image")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string GenerateQRImage { get; set; }

		[JsonPropertyName("code")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Code { get; set; }
	}
}
