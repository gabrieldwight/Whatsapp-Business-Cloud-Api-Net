using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ConversationAnalyticsResponse
    {
        [JsonProperty("conversation_analytics")]
        public ConversationAnalytics ConversationAnalytics { get; set; }
    }

    public class ConversationAnalytics
    {
        [JsonProperty("data")]
        public List<ConversationAnalyticsData> Data { get; set; }
    }

    public class ConversationAnalyticsData
    {
        [JsonProperty("data_points")]
        public List<ConversationAnalyticsDataPoint> DataPoints { get; set; }
    }

    public class ConversationAnalyticsDataPoint
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("conversation")]
        public long Conversation { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("conversation_type")]
        public string ConversationType { get; set; }

        [JsonProperty("conversation_direction")]
        public string ConversationDirection { get; set; }

        [JsonProperty("cost")]
        public double Cost { get; set; }
    }
}