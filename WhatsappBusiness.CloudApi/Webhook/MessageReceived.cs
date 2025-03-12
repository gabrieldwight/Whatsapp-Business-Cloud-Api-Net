using System.Collections.Generic;
using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Interfaces;
namespace WhatsappBusiness.CloudApi.Webhook;


public abstract class GenericMessage : IGenericMessage
{
    [JsonPropertyName("from")]
    public virtual string From { get; set; }

    [JsonPropertyName("id")]
    public virtual string Id { get; set; }

    [JsonPropertyName("timestamp")]
    public virtual string Timestamp { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("context")]
    public virtual MessageContext? Context { get; set; }

}

public class MessageReceived<TMessageType> where TMessageType : IGenericMessage
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("entry")]
    public List<MessageEntry<TMessageType>> Entry { get; set; }

}

public class MessageEntry<TMessageType>
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("changes")]
    public List<MessageChange<TMessageType>> Changes { get; set; }
}

public class MessageChange<TMessageType>
{
    [JsonPropertyName("value")]
    public MessageValue<TMessageType> Value { get; set; }

    [JsonPropertyName("field")]
    public string Field { get; set; }
}

public class MessageValue<TMessageType>
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; }

    [JsonPropertyName("metadata")]
    public MessageMetadata Metadata { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; }

    [JsonPropertyName("messages")]
    public List<TMessageType> Messages { get; set; }
}

public class MessageMetadata
{
    [JsonPropertyName("display_phone_number")]
    public string DisplayPhoneNumber { get; set; }

    [JsonPropertyName("phone_number_id")]
    public string PhoneNumberId { get; set; }
}



public class Contact
{
    [JsonPropertyName("profile")]
    public Profile Profile { get; set; }

    [JsonPropertyName("wa_id")]
    public string WaId { get; set; }
}

public class Profile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class MessageContext
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}