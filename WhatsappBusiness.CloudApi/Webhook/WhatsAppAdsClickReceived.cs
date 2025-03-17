using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{ 

    public class WhatsAppAdsClickMessage:GenericMessage
    {
        [JsonPropertyName("referral")]
        public Referral Referral { get; set; }

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
    
}