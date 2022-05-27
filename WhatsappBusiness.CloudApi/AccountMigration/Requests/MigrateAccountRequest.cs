using Newtonsoft.Json;

namespace WhatsappBusiness.CloudApi.AccountMigration.Requests
{
    public class MigrateAccountRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("backup")]
        public Backup Backup { get; set; }
    }

    public class Backup
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
