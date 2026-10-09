using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.Webhook;
using Xunit;

namespace WhatsappBusiness.CloudApi.Tests
{
    public class CloudApiSchemaTests
    {
        [Fact]
        public void MediaWebhookModelsDeserializeMediaUrls()
        {
            const string url = "https://example.com/media";

            Assert.Equal(url, JsonSerializer.Deserialize<WhatsappBusiness.CloudApi.Webhook.Image>($"{{\"url\":\"{url}\"}}").Url);
            Assert.Equal(url, JsonSerializer.Deserialize<WhatsappBusiness.CloudApi.Webhook.Video>($"{{\"url\":\"{url}\"}}").Url);
            Assert.Equal(url, JsonSerializer.Deserialize<Audio>($"{{\"url\":\"{url}\"}}").Url);
            Assert.Equal(url, JsonSerializer.Deserialize<WhatsappBusiness.CloudApi.Webhook.Document>($"{{\"url\":\"{url}\"}}").Url);
            Assert.Equal(url, JsonSerializer.Deserialize<Sticker>($"{{\"url\":\"{url}\"}}").Url);
        }

        [Fact]
        public void MessageStatusDeserializesOptionalConversationAndGroupFields()
        {
            const string json = "{\"id\":\"message-id\",\"status\":\"played\",\"timestamp\":\"1\",\"recipient_id\":\"group-id\",\"recipient_type\":\"group\",\"recipient_participant_id\":\"user-id\",\"recipient_identity_key_hash\":\"identity-hash\",\"biz_opaque_callback_data\":\"callback-data\"}";

            var status = JsonSerializer.Deserialize<MessageStatus>(json);

            Assert.Equal("played", status.Status);
            Assert.Equal("group", status.RecipientType);
            Assert.Equal("user-id", status.RecipientParticipantId);
            Assert.Equal("identity-hash", status.RecipientIdentityKeyHash);
            Assert.Equal("callback-data", status.BizOpaqueCallbackData);
            Assert.Null(status.Conversation);
        }

        [Fact]
        public void MessagingLimitFieldsDeserializeFromGraphApiResponses()
        {
            var waba = JsonSerializer.Deserialize<WABADetailsResponse>(
                "{\"id\":\"waba-id\",\"whatsapp_business_manager_messaging_limit\":\"TIER_250\"}");
            var phoneNumber = JsonSerializer.Deserialize<PhoneNumberByIdResponse>(
                "{\"id\":\"phone-id\",\"whatsapp_business_manager_messaging_limit\":\"TIER_250\",\"messaging_limit_tier\":\"TIER_250\"}");

            Assert.Equal("TIER_250", waba.WhatsAppBusinessManagerMessagingLimit);
            Assert.Equal("TIER_250", phoneNumber.WhatsAppBusinessManagerMessagingLimit);
            Assert.Equal("TIER_250", phoneNumber.MessagingLimitTier);
            Assert.Contains("whatsapp_business_manager_messaging_limit", WhatsAppBusinessRequestEndpoint.GetWABADetailsWithPortfolioMessagingLimit);
            Assert.Contains("messaging_limit_tier", WhatsAppBusinessRequestEndpoint.GetPhoneNumberByIdWithPortfolioMessagingLimit);
        }

        [Theory]
        [InlineData("v23.0", false)]
        [InlineData("v24.0", true)]
        [InlineData("v25.0", true)]
        public async Task GraphApiVersionControlsPortfolioMessagingLimitFields(string graphApiVersion, bool shouldIncludeFields)
        {
            var handler = new RecordingHandler();
            using (var httpClient = new HttpClient(handler)
            {
                BaseAddress = new System.Uri($"https://graph.facebook.com/{graphApiVersion}/")
            })
            {
                var client = new WhatsAppBusinessClient(
                    httpClient,
                    new WhatsAppBusinessCloudApiConfig { AccessToken = "test-token" });

                await client.GetWABADetailsAsync("waba-id");
                await client.GetWhatsAppBusinessAccountPhoneNumberByIdAsync("phone-id");
            }

            Assert.Equal(2, handler.RequestUris.Count);
            foreach (var requestUri in handler.RequestUris)
            {
                var containsLimitField = requestUri.Query.Contains("whatsapp_business_manager_messaging_limit");
                Assert.Equal(shouldIncludeFields, containsLimitField);
            }
        }

        [Fact]
        public void TextTemplateRequestSerializesTapTargetConfiguration()
        {
            var request = new TextTemplateMessageRequest
            {
                To = "15551234567",
                Template = new TextMessageTemplate
                {
                    Name = "promotion",
                    Language = new TextMessageLanguage { Code = "en" },
                    Components = new List<TextMessageComponent>
                    {
                        new TextMessageComponent
                        {
                            Type = "tap_target_configuration",
                            Parameters = new List<TextMessageParameter>
                            {
                                new TextMessageParameter
                                {
                                    Type = "tap_target_configuration",
                                    TapTargetConfiguration = new List<TapTargetConfigurationData>
                                    {
                                        new TapTargetConfigurationData
                                        {
                                            Url = "https://example.com",
                                            Title = "View offer"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            using (var document = JsonDocument.Parse(JsonSerializer.Serialize(request)))
            {
                var target = document.RootElement
                    .GetProperty("template")
                    .GetProperty("components")[0]
                    .GetProperty("parameters")[0]
                    .GetProperty("tap_target_configuration")[0];

                Assert.Equal("https://example.com", target.GetProperty("url").GetString());
                Assert.Equal("View offer", target.GetProperty("title").GetString());
            }
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            public List<System.Uri> RequestUris { get; } = new List<System.Uri>();

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestUris.Add(request.RequestUri);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"id\":\"entity\"}", Encoding.UTF8, "application/json")
                });
            }
        }
    }
}
