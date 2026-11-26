using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.PhoneNumbers.Requests
{
    public class UpdatePhoneNumberDisplayNameRequest
    {
        [JsonPropertyName("new_display_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NewDisplayName { get; set; }
    }
}
