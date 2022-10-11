using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Webhook
{
    /// <summary>
    /// A contact message you received from a customer
    /// </summary>
    public class ContactMessageReceived
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<ContactMessageEntry> Entry { get; set; }
    }

    public class ContactMessageEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<ContactMessageChange> Changes { get; set; }
    }

    public class ContactMessageChange
    {
        [JsonProperty("value")]
        public ContactMessageValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class ContactMessageValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public ContactMessageMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<ValueContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<ContactMessage> Messages { get; set; }
    }

    public class ValueContact
    {
        [JsonProperty("profile")]
        public ContactMessageProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class ContactMessageProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ContactMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("contacts")]
        public List<MessageContact> Contacts { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("context")]
        public ContactMessageContext? Context { get; set; }
    }

    public class MessageContact
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
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }
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

    public class Org
    {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Phone
    {
        [JsonProperty("phone")]
        public string PhonePhone { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Url
    {
        [JsonProperty("url")]
        public string UrlUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ContactMessageMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class ContactMessageContext
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}