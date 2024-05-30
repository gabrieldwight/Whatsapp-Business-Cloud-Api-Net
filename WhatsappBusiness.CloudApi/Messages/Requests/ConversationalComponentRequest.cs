using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class ConversationalComponentRequest
    {
        [JsonProperty("enable_welcome_message")]
        public bool EnableWelcomeMessage { get; set; }

        [JsonProperty("commands")]
        public List<ConversationalComponentCommand> Commands { get; set; }

        [JsonProperty("prompts")]
        public List<string> Prompts { get; set; }
    }

    public class ConversationalComponentCommand
    {
        [JsonProperty("command_name")]
        public string CommandName { get; set; }

        [JsonProperty("command_description")]
        public string CommandDescription { get; set; }
    }
}
