using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A message you received from a customer that is not supported.
    /// </summary>
    public class UnknownMessage : GenericMessage
    {

        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }

    }

    public class Error
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
    
}