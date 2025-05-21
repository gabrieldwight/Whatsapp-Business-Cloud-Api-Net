using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class GetBlockedUserResponse
	{
		[JsonPropertyName("data")]
		public List<BlockedUserData> Data { get; set; }

		[JsonPropertyName("paging")]
		public BlockedUserPaging Paging { get; set; }
	}

	public class BlockedUserData
	{
		[JsonPropertyName("block_users")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<BlockUser> BlockUsers { get; set; }

		[JsonPropertyName("messaging_product")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("wa_id")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string WaId { get; set; }
	}

	public class BlockUser : BaseUser
	{

	}

	public class BlockedUserPaging
	{
		[JsonPropertyName("cursors")]
		public BlockedUserCursors Cursors { get; set; }

		[JsonPropertyName("previous")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Previous { get; set; }

		[JsonPropertyName("next")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Next { get; set; }
	}

	public class BlockedUserCursors
	{
		[JsonPropertyName("after")]
		public string After { get; set; }

		[JsonPropertyName("before")]
		public string Before { get; set; }
	}
}
