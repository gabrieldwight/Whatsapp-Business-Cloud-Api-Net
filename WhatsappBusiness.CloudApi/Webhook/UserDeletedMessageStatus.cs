using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// Currently, the Cloud API does not support webhook status updates for deleted messages. If a user deletes a message, you will receive a webhook with an error code for an unsupported message type
    /// </summary>

    public class UserDeletedMessage : GenericMessage
    {

        [JsonPropertyName("errors")]
        public List<UserDeletedMessageError> Errors { get; set; }
    }

    public class UserDeletedMessageError
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
    
}