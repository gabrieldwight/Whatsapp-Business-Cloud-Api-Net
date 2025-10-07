using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class WhatsAppGroupJoinRequestResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("approved_join_requests")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> ApprovedJoinRequests { get; set; }

		[JsonPropertyName("rejected_join_requests")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<string> RejectedJoinRequests { get; set; }

		[JsonPropertyName("failed_join_requests")]
		public List<FailedJoinRequest> FailedJoinRequests { get; set; }

		[JsonPropertyName("errors")]
		public List<WhatsAppGroupJoinError> Errors { get; set; }
	}

	public class WhatsAppGroupJoinError
	{
		[JsonPropertyName("code")]
		public string Code { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("error_data")]
		public WhatsAppGroupJoinErrorData ErrorData { get; set; }
	}

	public class WhatsAppGroupJoinErrorData
	{
		[JsonPropertyName("details")]
		public string Details { get; set; }
	}

	public class FailedJoinRequest
	{
		[JsonPropertyName("join_request_id")]
		public string JoinRequestId { get; set; }

		[JsonPropertyName("errors")]
		public List<Error> Errors { get; set; }
	}
}
