using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class QRCodeMessageFilterResponse
    {
        [JsonPropertyName("data")]
        public List<QRCodeMessageResponse> Data { get; set; }
    }
}
