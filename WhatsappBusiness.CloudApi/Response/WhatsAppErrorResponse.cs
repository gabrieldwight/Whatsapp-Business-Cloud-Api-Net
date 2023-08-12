using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.Response
{
    public class WhatsAppErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("error_data")]
        public ErrorData ErrorData { get; set; }

        [JsonProperty("error_subcode")]
        public long? ErrorSubcode { get; set; }

        [JsonProperty("fbtrace_id")]
        public string FbtraceId { get; set; }
    }

    public class ErrorData
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }
    }
}