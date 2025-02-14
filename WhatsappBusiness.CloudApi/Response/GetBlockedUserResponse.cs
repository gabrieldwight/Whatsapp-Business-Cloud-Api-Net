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
		public List<BlockUser> BlockUsers { get; set; }
	}

	public class BlockUser : BaseUser
	{

	}

	public class BlockedUserPaging
	{
		[JsonPropertyName("cursors")]
		public BlockedUserCursors Cursors { get; set; }

		[JsonPropertyName("previous")]
		public string Previous { get; set; }

		[JsonPropertyName("next")]
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
