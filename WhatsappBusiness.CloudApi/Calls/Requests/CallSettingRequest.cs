using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Calls.Requests
{
	public class CallSettingRequest
	{
		[JsonPropertyName("calling")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Calling Calling { get; set; }

		[JsonPropertyName("storage_configuration")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public StorageConfiguration? StorageConfiguration { get; set; }
	}

	public class StorageConfiguration
	{
		[JsonPropertyName("status")]
		public string? Status { get; set; }

		[JsonPropertyName("data_localization_region")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? DataLocalizationRegion { get; set; }

		[JsonPropertyName("retention_minutes")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? RetentionMinutes { get; set; }
	}

	public class Calling
	{
		[JsonPropertyName("status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Status { get; set; }

		[JsonPropertyName("call_icon_visibility")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string CallIconVisibility { get; set; }

		[JsonPropertyName("call_icons")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CallIcons CallIcons { get; set; }

		[JsonPropertyName("call_hours")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CallHours CallHours { get; set; }

		[JsonPropertyName("callback_permission_status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string CallbackPermissionStatus { get; set; }

		[JsonPropertyName("sip")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Sip Sip { get; set; }

		[JsonPropertyName("audio")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CallingAudio Audio { get; set; }

		[JsonPropertyName("voicemail")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Voicemail Voicemail { get; set; }
	}

	public class CallIcons
	{
		[JsonPropertyName("restrict_to_user_countries")]
		public List<string> RestrictToUserCountries { get; set; }
	}

	public class CallHours
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("timezone_id")]
		public string TimezoneId { get; set; }

		[JsonPropertyName("weekly_operating_hours")]
		public List<WeeklyOperatingHour> WeeklyOperatingHours { get; set; }

		[JsonPropertyName("holiday_schedule")]
		public List<HolidaySchedule> HolidaySchedule { get; set; }
	}

	public class WeeklyOperatingHour
	{
		[JsonPropertyName("day_of_week")]
		public string DayOfWeek { get; set; }

		[JsonPropertyName("open_time")]
		public string OpenTime { get; set; }

		[JsonPropertyName("close_time")]
		public string CloseTime { get; set; }
	}

	public class HolidaySchedule
	{
		[JsonPropertyName("date")]
		public string Date { get; set; }

		[JsonPropertyName("start_time")]
		public string StartTime { get; set; }

		[JsonPropertyName("end_time")]
		public string EndTime { get; set; }
	}

	public class Sip
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("servers")]
		public List<Server> Servers { get; set; }
	}

	public class Server
	{
		[JsonPropertyName("hostname")]
		public string Hostname { get; set; }

		[JsonPropertyName("port")]
		public string Port { get; set; }

		[JsonPropertyName("request_uri_user_params")]
		public RequestUriUserParams RequestUriUserParams { get; set; }
	}

	public class RequestUriUserParams
	{
		[JsonPropertyName("KEY1")]
		public string Key1 { get; set; }

		[JsonPropertyName("KEY2")]
		public string Key2 { get; set; }
	}

	public class CallingAudio
	{
		[JsonPropertyName("additional_codecs")]
		public List<string> AdditionalCodecs { get; set; }
	}

	public class Voicemail
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("triggers")]
		public List<string> Triggers { get; set; }

		[JsonPropertyName("audio")]
		public VoicemailAudio Audio { get; set; }
	}

	public class VoicemailAudio
	{
		[JsonPropertyName("default")]
		public VoicemailDefault Default { get; set; }
	}

	public class VoicemailDefault
	{
		[JsonPropertyName("announcement_media_id")]
		public long AnnouncementMediaId { get; set; }

		[JsonPropertyName("timeout_seconds")]
		public long TimeoutSeconds { get; set; }
	}
}
