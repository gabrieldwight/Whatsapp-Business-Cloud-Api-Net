using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.AccountMigration.Requests
{
    public class MigrateAccountRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("pin")]
        public string Pin { get; set; }

        [JsonPropertyName("backup")]
        public Backup Backup { get; set; }
    }

    public class Backup
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
