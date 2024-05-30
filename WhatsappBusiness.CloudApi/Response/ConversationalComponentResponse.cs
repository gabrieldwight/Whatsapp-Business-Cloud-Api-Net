using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class ConversationalComponentResponse
    {
        [JsonProperty("conversational_automation")]
        public ConversationalAutomation ConversationalAutomation { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ConversationalAutomation
    {
        [JsonProperty("enable_welcome_message")]
        public bool EnableWelcomeMessage { get; set; }

        [JsonProperty("prompts")]
        public List<string> Prompts { get; set; }

        [JsonProperty("commands")]
        public List<ConversationalCommand> Commands { get; set; }
    }

    public class ConversationalCommand
    {
        [JsonProperty("command_name")]
        public string CommandName { get; set; }

        [JsonProperty("command_description")]
        public string CommandDescription { get; set; }
    }
}
