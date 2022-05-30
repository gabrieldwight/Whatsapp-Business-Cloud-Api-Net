using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Messages.Requests
{
    public class ContactMessageRequest
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; private set; } = "whatsapp";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; private set; } = "contacts";

        [JsonProperty("contacts")]
        public List<ContactData> Contacts { get; set; }
    }

    public class ContactData
    {
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("emails")]
        public List<Email> Emails { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("org")]
        public Org Org { get; set; }

        [JsonProperty("phones")]
        public List<Phone> Phones { get; set; }

        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }
    }

    public class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Email
    {
        [JsonProperty("email")]
        public string EmailEmail { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Name
    {
        [JsonProperty("formatted_name")]
        public string FormattedName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }
    }

    public partial class Org
    {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Phone
    {
        [JsonProperty("phone")]
        public string PhonePhone { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("wa_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WaId { get; set; }
    }

    public partial class Url
    {
        [JsonProperty("url")]
        public string UrlUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}