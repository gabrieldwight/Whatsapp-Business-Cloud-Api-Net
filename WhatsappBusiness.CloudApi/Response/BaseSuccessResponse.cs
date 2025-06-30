using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class BaseSuccessResponse
    {
		[JsonPropertyName("messaging_product")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
