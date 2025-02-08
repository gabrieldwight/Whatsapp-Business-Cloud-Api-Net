using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ConversationalComponentResponse
    {
        [JsonPropertyName("conversational_automation")]
        public ConversationalAutomation ConversationalAutomation { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class ConversationalAutomation
    {
        [JsonPropertyName("enable_welcome_message")]
        public bool EnableWelcomeMessage { get; set; }

        [JsonPropertyName("prompts")]
        public List<string> Prompts { get; set; }

        [JsonPropertyName("commands")]
        public List<ConversationalCommand> Commands { get; set; }
    }

    public class ConversationalCommand
    {
        [JsonPropertyName("command_name")]
        public string CommandName { get; set; }

        [JsonPropertyName("command_description")]
        public string CommandDescription { get; set; }
    }
}
