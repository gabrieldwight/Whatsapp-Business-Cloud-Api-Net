using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Groups.Requests
{
	public class GroupJoinRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("join_requests")]
		public List<string> JoinRequests { get; set; }

		[JsonPropertyName("rejected_join_requests")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> RejectedJoinRequests { get; set; }
	}
}
