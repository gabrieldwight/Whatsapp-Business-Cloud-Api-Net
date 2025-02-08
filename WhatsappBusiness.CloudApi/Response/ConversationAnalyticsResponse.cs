using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ConversationAnalyticsResponse
    {
        [JsonPropertyName("conversation_analytics")]
        public ConversationAnalytics ConversationAnalytics { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

    public class ConversationAnalytics
    {
        [JsonPropertyName("data")]
        public List<ConversationAnalyticsData> Data { get; set; }
    }

    public class ConversationAnalyticsData
    {
        [JsonPropertyName("data_points")]
        public List<ConversationAnalyticsDataPoint> DataPoints { get; set; }
    }

    public class ConversationAnalyticsDataPoint
    {
        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("end")]
        public long End { get; set; }

        [JsonPropertyName("conversation")]
        public long Conversation { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("conversation_type")]
        public string ConversationType { get; set; }

        [JsonPropertyName("conversation_direction")]
        public string ConversationDirection { get; set; }

        [JsonPropertyName("cost")]
        public double Cost { get; set; }
    }
}