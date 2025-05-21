using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhatsappBusiness.CloudApi.Response;

public class WhatsappBusinessEncryptionResponse
{
    [JsonPropertyName("data")] 
    public List<WhatsappBusinessEncryptionData> Data { get; set; }
}

public class WhatsappBusinessEncryptionData
{
    [JsonPropertyName("business_public_key")]
    public string BusinessPublicKey { get; set; }

    [JsonPropertyName("business_public_key_signature_status")]
    public string BusinessPublicKeySignatureStatus { get; set; }
}