using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{ 

    public class WhatsAppAdsClickMessage : GenericMessage
    {
        [JsonPropertyName("referral")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Referral Referral { get; set; }

        [JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public WhatsAppAdsClickText? Text { get; set; }

		[JsonPropertyName("audio")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public WhatsAppAdsClickAudio? Audio { get; set; }

        [JsonPropertyName("image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WhatsAppAdsClickImage? Image { get; set; }

        [JsonPropertyName("video")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WhatsAppAdsClickVideo? Video { get; set; }
	}

    public class Referral
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

		[JsonPropertyName("ctwa_clid")]
		public string CtwaClId { get; set; }

		[JsonPropertyName("welcome_message")]
		public ReferralWelcomeMessage WelcomeMessage { get; set; }
	}

    public class WhatsAppAdsClickText
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class ReferralWelcomeMessage
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
	}

	public class WhatsAppAdsClickAudio : Audio
	{
		
	}

    public class WhatsAppAdsClickImage : Image
    {

	}

    public class WhatsAppAdsClickVideo : Video
    {

	}
}