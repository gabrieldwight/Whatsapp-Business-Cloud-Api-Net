using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class WhatsAppAdsClickReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<WhatsAppAdsClickEntry> Entry { get; set; }
    }

    public class WhatsAppAdsClickEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<WhatsAppAdsClickChange> Changes { get; set; }
    }

    public class WhatsAppAdsClickChange
    {
        [JsonProperty("value")]
        public WhatsAppAdsClickValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class WhatsAppAdsClickValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public WhatsAppAdsClickMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<WhatsAppAdsClickContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<WhatsAppAdsClickMessage> Messages { get; set; }
    }

    public class WhatsAppAdsClickContact
    {
        [JsonProperty("profile")]
        public WhatsAppAdsClickProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class WhatsAppAdsClickProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class WhatsAppAdsClickMessage
    {
        [JsonProperty("referral")]
        public Referral Referral { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public WhatsAppAdsClickText Text { get; set; }
    }

    public partial class Referral
    {
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        [JsonProperty("source_id")]
        public string SourceId { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("video_url")]
        public string VideoUrl { get; set; }

        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }

    public class WhatsAppAdsClickText
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class WhatsAppAdsClickMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}