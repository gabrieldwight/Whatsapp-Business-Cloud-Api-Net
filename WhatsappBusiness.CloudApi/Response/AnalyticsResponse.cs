using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class AnalyticsResponse
    {
        [JsonPropertyName("analytics")]
        public Analytics Analytics { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Analytics
    {
        [JsonPropertyName("phone_numbers")]
        public List<string> PhoneNumbers { get; set; }

        [JsonPropertyName("country_codes")]
        public List<string> CountryCodes { get; set; }

        [JsonPropertyName("granularity")]
        public string Granularity { get; set; }

        [JsonPropertyName("data_points")]
        public List<AnalyticsDataPoint> DataPoints { get; set; }
    }

    public class AnalyticsDataPoint
    {
        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("end")]
        public long End { get; set; }

        [JsonPropertyName("sent")]
        public long Sent { get; set; }

        [JsonPropertyName("delivered")]
        public long Delivered { get; set; }
    }
}