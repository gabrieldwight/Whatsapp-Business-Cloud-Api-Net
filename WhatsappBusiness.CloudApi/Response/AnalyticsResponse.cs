using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class AnalyticsResponse
    {
        [JsonProperty("analytics")]
        public Analytics Analytics { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Analytics
    {
        [JsonProperty("phone_numbers")]
        public List<string> PhoneNumbers { get; set; }

        [JsonProperty("country_codes")]
        public List<string> CountryCodes { get; set; }

        [JsonProperty("granularity")]
        public string Granularity { get; set; }

        [JsonProperty("data_points")]
        public List<AnalyticsDataPoint> DataPoints { get; set; }
    }

    public class AnalyticsDataPoint
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("sent")]
        public long Sent { get; set; }

        [JsonProperty("delivered")]
        public long Delivered { get; set; }
    }
}