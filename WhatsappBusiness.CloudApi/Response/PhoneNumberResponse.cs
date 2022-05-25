using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappBusiness.CloudApi.Response
{
    public class PhoneNumberResponse
    {
        [JsonProperty("data")]
        public List<PhoneNumberData> Data { get; set; }
    }

    public class PhoneNumberData
    {
        [JsonProperty("verified_name")]
        public string VerifiedName { get; set; }

        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quality_rating")]
        public string QualityRating { get; set; }
    }
}
