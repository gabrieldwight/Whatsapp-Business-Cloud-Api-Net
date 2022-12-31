using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class QRCodeMessageResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("prefilled_message")]
        public string PrefilledMessage { get; set; }

        [JsonProperty("deep_link_url")]
        public string DeepLinkUrl { get; set; }

        [JsonProperty("qr_image_url")]
        public string QrImageUrl { get; set; }
    }
}
