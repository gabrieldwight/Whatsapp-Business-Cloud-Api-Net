using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.PhoneNumbers.Requests
{
    public class UpdateOfficialBusinessAccountStatusRequest
    {
        [JsonPropertyName("business_website_url")]
        public string? BusinessWebsiteUrl { get; set; }

        [JsonPropertyName("primary_country_of_operation")]
        public string? PrimaryCountryOfOperation { get; set; }

        [JsonPropertyName("primary_language")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PrimaryLanguage { get; set; }

        [JsonPropertyName("parent_business_or_brand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ParentBusinessOrBrand { get; set; }

        [JsonPropertyName("supporting_links")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? SupportingLinks { get; set; }

        [JsonPropertyName("additional_supporting_information")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdditionalSupportingInformation { get; set; }
    }
}
