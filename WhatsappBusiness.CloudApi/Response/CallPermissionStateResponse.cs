using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response
{
	public class CallPermissionStateResponse
	{
		[JsonPropertyName("messaging_product")]
		public string MessagingProduct { get; set; }

		[JsonPropertyName("permission")]
		public Permission Permission { get; set; }

		[JsonPropertyName("actions")]
		public List<CallPermissionAction> Actions { get; set; }
	}

	public class Permission
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("expiration_time")]
		public int ExpirationTime { get; set; }
	}

	public class CallPermissionAction
	{
		[JsonPropertyName("action_name")]
		public string ActionName { get; set; }

		[JsonPropertyName("can_perform_action")]
		public bool CanPerformAction { get; set; }

		[JsonPropertyName("limits")]
		public List<Limit> Limits { get; set; }
	}

	public class Limit
	{
		[JsonPropertyName("time_period")]
		public string TimePeriod { get; set; }

		[JsonPropertyName("max_allowed")]
		public int MaxAllowed { get; set; }

		[JsonPropertyName("current_usage")]
		public int CurrentUsage { get; set; }

		[JsonPropertyName("limit_expiration_time")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int? LimitExpirationTime { get; set; }
	}
}
