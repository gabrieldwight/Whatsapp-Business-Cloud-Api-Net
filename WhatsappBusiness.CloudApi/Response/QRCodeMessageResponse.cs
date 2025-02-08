using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class QRCodeMessageResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("prefilled_message")]
        public string PrefilledMessage { get; set; }

        [JsonPropertyName("deep_link_url")]
        public string DeepLinkUrl { get; set; }

        [JsonPropertyName("qr_image_url")]
        public string QrImageUrl { get; set; }
    }
}
