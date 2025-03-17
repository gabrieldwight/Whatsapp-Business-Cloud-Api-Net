using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A contact message you received from a customer
    /// </summary>
    
    

    public class ContactMessage: GenericMessage
    {
        [JsonPropertyName("contacts")]
        public List<MessageContact> Contacts { get; set; }

        [JsonPropertyName("context")]
        public MessageContext? Context { get; set; }
    }

    public class MessageContact
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
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }
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

    public class Org
    {
        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class Phone
    {
        [JsonPropertyName("phone")]
        public string PhonePhone { get; set; }

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Url
    {
        [JsonPropertyName("url")]
        public string UrlUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}