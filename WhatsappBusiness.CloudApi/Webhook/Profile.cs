using System.Text.Json.Serialization;
namespace WhatsappBusiness.CloudApi.Webhook;
public class Profile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}