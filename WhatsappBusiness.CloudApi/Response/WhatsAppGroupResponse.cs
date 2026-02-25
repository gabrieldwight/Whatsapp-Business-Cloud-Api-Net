using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class WhatsAppGroupResponse
	{
		[JsonPropertyName("data")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<WhatsAppGroupData> Data { get; set; }

		[JsonPropertyName("paging")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public WhatsAppGroupPaging Paging { get; set; }
	}

	public class WhatsAppGroupCursors
	{
		[JsonPropertyName("before")]
		public string Before { get; set; }

		[JsonPropertyName("after")]
		public string After { get; set; }
	}

	public class WhatsAppGroupData
	{
		[JsonPropertyName("join_request_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string JoinRequestId { get; set; }

		[JsonPropertyName("wa_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string WaId { get; set; }

		[JsonPropertyName("user_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string UserId { get; set; }

		[JsonPropertyName("parent_user_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string ParentUserId { get; set; }

		[JsonPropertyName("username")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Username { get; set; }

		[JsonPropertyName("creation_timestamp")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string CreationTimestamp { get; set; }

		[JsonPropertyName("groups")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<WhatsAppGroup> Groups { get; set; }
	}

	public class WhatsAppGroup
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("subject")]
		public string Subject { get; set; }

		[JsonPropertyName("created_at")]
		public string CreatedAt { get; set; }
	}

	public class WhatsAppGroupPaging
	{
		[JsonPropertyName("cursors")]
		public WhatsAppGroupCursors Cursors { get; set; }

		[JsonPropertyName("previous")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Previous { get; set; }

		[JsonPropertyName("next")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Next { get; set; }
	}
}
