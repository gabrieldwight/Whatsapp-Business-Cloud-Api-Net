using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class ContactMessageRequest
    {
        [JsonPropertyName("messaging_product")]
        [JsonInclude]
		public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
		[JsonInclude]
		public string Type { get; private set; } = "contacts";

        [JsonPropertyName("contacts")]
        public List<ContactData> Contacts { get; set; }

		[JsonPropertyName("biz_opaque_callback_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BizOpaqueCallbackData { get; set; }
	}

    public class ContactData
    {
        [JsonPropertyName("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("emails")]
        public List<Email> Emails { get; set; }

        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("org")]
        public Org Org { get; set; }

        [JsonPropertyName("phones")]
        public List<Phone> Phones { get; set; }

        [JsonPropertyName("urls")]
        public List<Url> Urls { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Email
    {
        [JsonPropertyName("email")]
        public string EmailEmail { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("formatted_name")]
        public string FormattedName { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("suffix")]
        public string Suffix { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }
    }

    public partial class Org
    {
        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public partial class Phone
    {
        [JsonPropertyName("phone")]
        public string PhonePhone { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("wa_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string WaId { get; set; }
    }

    public partial class Url
    {
        [JsonPropertyName("url")]
        public string UrlUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}