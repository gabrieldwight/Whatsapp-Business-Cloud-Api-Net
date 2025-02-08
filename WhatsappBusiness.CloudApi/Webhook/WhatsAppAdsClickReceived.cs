using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class WhatsAppAdsClickReceived
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<WhatsAppAdsClickEntry> Entry { get; set; }
    }

    public class WhatsAppAdsClickEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("changes")]
        public List<WhatsAppAdsClickChange> Changes { get; set; }
    }

    public class WhatsAppAdsClickChange
    {
        [JsonPropertyName("value")]
        public WhatsAppAdsClickValue Value { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }

    public class WhatsAppAdsClickValue
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("metadata")]
        public WhatsAppAdsClickMetadata Metadata { get; set; }

        [JsonPropertyName("contacts")]
        public List<WhatsAppAdsClickContact> Contacts { get; set; }

        [JsonPropertyName("messages")]
        public List<WhatsAppAdsClickMessage> Messages { get; set; }
    }

    public class WhatsAppAdsClickContact
    {
        [JsonPropertyName("profile")]
        public WhatsAppAdsClickProfile Profile { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }
    }

    public class WhatsAppAdsClickProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class WhatsAppAdsClickMessage
    {
        [JsonPropertyName("referral")]
        public Referral Referral { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public WhatsAppAdsClickText Text { get; set; }
    }

    public partial class Referral
    {
        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; }

        [JsonPropertyName("source_id")]
        public string SourceId { get; set; }

        [JsonPropertyName("source_type")]
        public string SourceType { get; set; }

        [JsonPropertyName("headline")]
        public string Headline { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("video_url")]
        public string VideoUrl { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }

    public class WhatsAppAdsClickText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class WhatsAppAdsClickMetadata
    {
        [JsonPropertyName("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonPropertyName("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}