using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class QRCodeMessageFilterResponse
    {
        [JsonProperty("data")]
        public List<QRCodeMessageResponse> Data { get; set; }
    }
}
