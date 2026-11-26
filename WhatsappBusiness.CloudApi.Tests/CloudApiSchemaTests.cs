using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.Calls.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.PhoneNumbers.Requests;
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

        [Fact]
        public void DefaultGraphApiVersionUsesLatestSupportedVersion()
        {
            Assert.Equal("https://graph.facebook.com/v27.0/", WhatsAppBusinessRequestEndpoint.BaseAddress.AbsoluteUri);
        }

        [Fact]
        public void PhoneNumberResponseDeserializesDisplayNameAndOBAFields()
        {
            var phoneNumber = JsonSerializer.Deserialize<PhoneNumberByIdResponse>(
                "{\"id\":\"phone-id\",\"name_status\":\"APPROVED\",\"code_verification_status\":\"VERIFIED\",\"official_business_account\":{\"oba_status\":\"PENDING\",\"status_message\":\"Under review\"}}");

            Assert.Equal("APPROVED", phoneNumber.NameStatus);
            Assert.Equal("VERIFIED", phoneNumber.CodeVerificationStatus);
            Assert.Equal("PENDING", phoneNumber.OfficialBusinessAccount.Status);
            Assert.Equal("Under review", phoneNumber.OfficialBusinessAccount.StatusMessage);
        }

        [Theory]
        [InlineData("v23.0", false)]
        [InlineData("v24.0", true)]
        [InlineData("v25.0", true)]
        [InlineData("v27.0", true)]
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
        public async Task PhoneNumberNameAndOBAOperationsUseDocumentedPathsAndPayloads()
        {
            var handler = new RecordingHandler("{\"success\":true,\"id\":\"phone-id\",\"oba_status\":\"PENDING\",\"status_message\":\"Under review\"}");
            using (var httpClient = new HttpClient(handler)
            {
                BaseAddress = new System.Uri("https://graph.facebook.com/v27.0/")
            })
            {
                var client = new WhatsAppBusinessClient(
                    httpClient,
                    new WhatsAppBusinessCloudApiConfig { AccessToken = "test-token" });

                await client.GetOfficialBusinessAccountStatusAsync("phone-id");
                await client.UpdateOfficialBusinessAccountStatusAsync("phone-id", new UpdateOfficialBusinessAccountStatusRequest
                {
                    BusinessWebsiteUrl = "https://example.com",
                    PrimaryCountryOfOperation = "US"
                });
                await client.UpdateWhatsAppBusinessPhoneNumberDisplayNameAsync("phone-id", new UpdatePhoneNumberDisplayNameRequest
                {
                    NewDisplayName = "Example Business"
                });
            }

            Assert.Equal(3, handler.RequestUris.Count);
            Assert.Equal("/v27.0/phone-id/official_business_account", handler.RequestUris[0].AbsolutePath);
            Assert.Equal(HttpMethod.Get, handler.RequestMethods[0]);
            Assert.Equal("/v27.0/phone-id/official_business_account", handler.RequestUris[1].AbsolutePath);
            Assert.Equal(HttpMethod.Post, handler.RequestMethods[1]);
            Assert.Contains("\"business_website_url\":\"https://example.com\"", handler.RequestBodies[1]);
            Assert.Equal("/v27.0/phone-id", handler.RequestUris[2].AbsolutePath);
            Assert.Equal(HttpMethod.Post, handler.RequestMethods[2]);
            Assert.Contains("\"new_display_name\":\"Example Business\"", handler.RequestBodies[2]);
        }

        [Fact]
        public async Task StorageConfigurationAndMediaTtlAreSentInTheirRequests()
        {
            var handler = new RecordingHandler("{\"success\":true,\"id\":\"entity\"}");
            using (var httpClient = new HttpClient(handler)
            {
                BaseAddress = new System.Uri("https://graph.facebook.com/v27.0/")
            })
            {
                var client = new WhatsAppBusinessClient(
                    httpClient,
                    new WhatsAppBusinessCloudApiConfig
                    {
                        AccessToken = "test-token",
                        WhatsAppBusinessPhoneNumberId = "phone-id"
                    });

                await client.ConfigureWhatsAppCallSettingsAsync(new CallSettingRequest
                {
                    StorageConfiguration = new StorageConfiguration
                    {
                        Status = "NO_STORAGE_ENABLED",
                        RetentionMinutes = 60
                    }
                });
                await client.UploadMediaAsync(new UploadMediaDataRequest
                {
                    FileName = "image.jpg",
                    Type = "image/jpeg",
                    Data = new byte[] { 1, 2, 3 },
                    TtlMinutes = 120
                });
            }

            Assert.Equal("/v27.0/phone-id/settings", handler.RequestUris[0].AbsolutePath);
            Assert.Contains("\"storage_configuration\":{\"status\":\"NO_STORAGE_ENABLED\",\"retention_minutes\":60}", handler.RequestBodies[0]);
            Assert.Contains("name=ttl_minutes", handler.RequestBodies[1]);
            Assert.Contains("120", handler.RequestBodies[1]);
        }

        [Fact]
        public void UserPreferencesWebhookDeserializesOptionalUserIdentifiers()
        {
            const string json = "{\"object\":\"whatsapp_business_account\",\"entry\":[{\"id\":\"waba-id\",\"changes\":[{\"field\":\"user_preferences\",\"value\":{\"messaging_product\":\"whatsapp\",\"metadata\":{\"display_phone_number\":\"15551234567\",\"phone_number_id\":\"phone-id\"},\"user_preferences\":[{\"detail\":\"User requested to stop marketing messages\",\"category\":\"marketing_messages\",\"value\":\"stop\",\"timestamp\":1731705721}]} }]}]}";

            var webhook = JsonSerializer.Deserialize<MessageReceived<TextMessage>>(json);
            var preference = webhook.Entry[0].Changes[0].Value.UserPreferences[0];

            Assert.Equal("marketing_messages", preference.Category);
            Assert.Equal("stop", preference.Value);
            Assert.Equal(1731705721, preference.Timestamp);
            Assert.Null(preference.UserId);
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
            private readonly string _responseContent;

            public RecordingHandler(string responseContent = "{\"id\":\"entity\"}")
            {
                _responseContent = responseContent;
            }

            public List<System.Uri> RequestUris { get; } = new List<System.Uri>();
            public List<HttpMethod> RequestMethods { get; } = new List<HttpMethod>();
            public List<string> RequestBodies { get; } = new List<string>();

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestUris.Add(request.RequestUri);
                RequestMethods.Add(request.Method);
                RequestBodies.Add(request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync());
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
