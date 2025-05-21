using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class BlockUserResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("block_users")]
		public BlockUsers BlockUsers { get; set; }

		[JsonPropertyName("error")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Error Error { get; set; }
	}

	public class Data
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }
	}

	public class BlockUsers
	{
		[JsonPropertyName("added_users")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<AddedUser> AddedUsers { get; set; }

		[JsonPropertyName("failed_users")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<FailedUser> FailedUsers { get; set; }

		[JsonPropertyName("removed_users")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<RemovedUser> RemovedUsers { get; set; }
	}

	public class AddedUser : BaseUser
	{

	}

	public class FailedUser : BaseUser
	{
		[JsonPropertyName("errors")]
		public List<ErrorElement> Errors { get; set; }
	}

	public class RemovedUser : BaseUser
	{

	}

	public partial class ErrorElement
	{
		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("code")]
		public long Code { get; set; }

		[JsonPropertyName("error_data")]
		public BlockUserErrorData ErrorData { get; set; }
	}

	public class BlockUserErrorData
	{
		[JsonPropertyName("details")]
		public string Details { get; set; }
	}

	public class BaseUser
	{
		[JsonPropertyName("input")]
		public string Input { get; set; }

		[JsonPropertyName("wa_id")]
		public string WaId { get; set; }
	}
}
