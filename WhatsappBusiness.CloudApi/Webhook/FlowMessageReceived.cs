using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
	public class FlowMessageReceived
	{
		[JsonProperty("messages")]
		public List<FlowMessage> Messages { get; set; }
	}

	public class FlowMessage
	{
		[JsonProperty("context")]
		public FlowContext Context { get; set; }

		[JsonProperty("from")]
		public string From { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("interactive")]
		public FlowInteractive Interactive { get; set; }

		[JsonProperty("timestamp")]
		public string Timestamp { get; set; }
	}

	public class FlowContext
	{
		[JsonProperty("from")]
		public string From { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }
	}

	public class FlowInteractive
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("nfm_reply")]
		public NfmReply NfmReply { get; set; }
	}

	public partial class NfmReply
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }

		[JsonProperty("response_json")]
		public string ResponseJson { get; set; }
	}
}
