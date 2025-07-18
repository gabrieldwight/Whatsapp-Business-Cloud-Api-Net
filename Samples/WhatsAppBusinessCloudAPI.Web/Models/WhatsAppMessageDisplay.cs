namespace WhatsAppBusinessCloudAPI.Web.Models
{
    public class WhatsAppMessageDisplay
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string MessageType { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public string RawJson { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
