using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Groups.Requests
{
	public class GroupRequest
	{
		[JsonPropertyName("messaging_product")]
		[JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

		[JsonPropertyName("subject")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Subject { get; set; }

		[JsonPropertyName("description")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Description { get; set; }

		[JsonPropertyName("profile_picture_file")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string profilePictureFile { get; set; }

		[JsonPropertyName("join_approval_mode")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string JoinApprovalMode { get; set; }

		[JsonPropertyName("participants")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<Participant> Participants { get; set; }
	}

	public class Participant
	{
		[JsonPropertyName("user")]
		public string User { get; set; }
	}
}
