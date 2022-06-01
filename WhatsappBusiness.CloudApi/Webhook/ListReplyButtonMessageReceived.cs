using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappBusiness.CloudApi.Webhook
{
    public class ListReplyButtonMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ListReplyButtonMessageEntry> Entry { get; set; }
    }

    public class ListReplyButtonMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ListReplyButtonMessageChange> Changes { get; set; }
    }

    public class ListReplyButtonMessageChange
    {
        [JsonProperty("value")]
        public ListReplyButtonMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ListReplyButtonMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ListReplyButtonMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ListReplyButtonMessageContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ListReplyButtonMessage> Messages { get; set; }
    }

    public class ListReplyButtonMessageContact
    {
        [JsonProperty("profile")]
        public ListReplyButtonMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ListReplyButtonMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ListReplyButtonMessage
    {
        [JsonProperty("context")]
        public ListReplyButtonMessageContext Context { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("interactive")]
        public Interactive Interactive { get; set; }
    }

    public class ListReplyButtonMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Interactive
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("list_reply")]
        public ListReply ListReply { get; set; }
    }

    public partial class ListReply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class ListReplyButtonMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }
}