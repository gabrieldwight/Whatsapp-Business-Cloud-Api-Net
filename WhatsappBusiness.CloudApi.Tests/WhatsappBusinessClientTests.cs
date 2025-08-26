using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Templates;
using WhatsappBusiness.CloudApi.OAuth.Requests;
using WhatsappBusiness.CloudApi.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WhatsappBusiness.CloudApi.Tests
{
    public class WhatsappBusinessClientTests : IClassFixture<TestSetup>
    {
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        private readonly EmbeddedSignupConfiguration _embeddedSignupConfig;
        private readonly List<WhatsAppBusinessCloudApiConfig> _sharedWhatsAppConfigs;
        private readonly WhatsAppBusinessClient _client;
        private readonly List<WhatsAppBusinessClient> _sharedClients = new List<WhatsAppBusinessClient>();

        public WhatsappBusinessClientTests(TestSetup testSetup)
        {
            var configuration = testSetup.ServiceProvider.GetRequiredService<IConfiguration>();

            _whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
            _whatsAppConfig.WhatsAppBusinessPhoneNumberId = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessPhoneNumberId"];
            _whatsAppConfig.WhatsAppBusinessAccountId = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessAccountId"];
            _whatsAppConfig.WhatsAppBusinessId = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessId"];
            _whatsAppConfig.AccessToken = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
            _whatsAppConfig.AppName = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AppName"];
            _whatsAppConfig.Version = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["Version"];
            _whatsAppConfig.WebhookVerifyToken = configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WebhookVerifyToken"];

            _embeddedSignupConfig = new EmbeddedSignupConfiguration();
            _embeddedSignupConfig.AppId = configuration.GetSection("EmbeddedSignupConfiguration")["AppId"];
            _embeddedSignupConfig.AppSecret = configuration.GetSection("EmbeddedSignupConfiguration")["AppSecret"];
            _embeddedSignupConfig.ConfigurationId = configuration.GetSection("EmbeddedSignupConfiguration")["ConfigurationId"];
            _embeddedSignupConfig.GraphApiVersion = configuration.GetSection("EmbeddedSignupConfiguration")["GraphApiVersion"];
            _embeddedSignupConfig.BaseUrl = configuration.GetSection("EmbeddedSignupConfiguration")["BaseUrl"];

            var factory = new WhatsAppBusinessClientFactory();
            // Initialize WhatsApp client with primary configuration
            _client = factory.Create(_whatsAppConfig);

            // Initialize shared WhatsApp configurations
            _sharedWhatsAppConfigs = new List<WhatsAppBusinessCloudApiConfig>();
            var sharedConfigsSection = configuration.GetSection("SharedWhatsAppBusinessCloudApiConfigurations");

            foreach (var configSection in sharedConfigsSection.GetChildren())
            {
                var sharedConfig = new WhatsAppBusinessCloudApiConfig();
                sharedConfig.WhatsAppBusinessPhoneNumberId = configSection["WhatsAppBusinessPhoneNumberId"];
                sharedConfig.WhatsAppBusinessAccountId = configSection["WhatsAppBusinessAccountId"];
                sharedConfig.WhatsAppBusinessId = configSection["WhatsAppBusinessId"];
                sharedConfig.AccessToken = configSection["AccessToken"];
                sharedConfig.AppName = configSection["AppName"];
                sharedConfig.Version = configSection["Version"];
                sharedConfig.WebhookVerifyToken = configSection["WebhookVerifyToken"];

                _sharedWhatsAppConfigs.Add(sharedConfig);
                var sharedClient = factory.Create(sharedConfig);
                _sharedClients.Add(sharedClient);
            }
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task CreateTemplateMessageAsync_UsingPositionalParameters_ShouldReturnCorrectStatus()
        {
            // Arrange
            var firstNameExampleText = "John";
            var companyNameExampleText = "My Company";
            var templateCategory = "MARKETING"; // or "UTILITY" or "AUTHENTICATION"
            var request = new BaseCreateTemplateMessageRequest
            {
                Name = $"test_template_{DateTime.UtcNow:yyyyMMddHHmmss}",
                Category = templateCategory,
                Language = "en_US",
                Components = new List<object>
                {
                    new TemplateComponent
                    {
                        Type = "HEADER",
                        Format = "TEXT",
                        Text = "{{1}} Intro",
                        Example = new TemplateComponentParameterExample
                        {
                            HeaderText = new List<string> { companyNameExampleText }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "BODY",
                        Text = "Hey {{1}}! {{2}} would love for you to schedule an appointment with us.",
                        Example = new TemplateComponentParameterExample
                        {
                            BodyText = new List<List<string>>
                            {
                                new List<string>{ firstNameExampleText, companyNameExampleText }
                            }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "FOOTER",
                        Text = "Reply STOP to unsubscribe."
                    },
                    new TemplateComponent
                    {
                        Type = "BUTTONS",
                        Buttons = new List<TemplateComponentButton>
                        {
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "YES"
                            },
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "NO"
                            }
                        }
                    }
                }
            };

            // Act
            var response = await _client.CreateTemplateMessageAsync(_whatsAppConfig.WhatsAppBusinessAccountId, request);

            // Assert
            response.Status.Should().NotBeNullOrEmpty();
            response.Status.Should().BeOneOf("PENDING", "APPROVED");
            response.Id.Should().NotBeNullOrEmpty();
            response.Category.Should().Be(templateCategory);
        }

        [Fact(Skip = "WhatsApp Manager -> Manage Template -> Edit Template: Doesn't seem to like named parameters. It accepts only positional parameters.")]
        //[Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task CreateTemplateMessageAsync_UsingNamedParameters_ShouldReturnCorrectStatus()
        {
            // Arrange
            var firstNameParameterName = "first_name";
            var firstNameExampleText = "John";
            var companyNameParameterName = "company_name";
            var companyNameExampleText = "My Company";
            var templateCategory = "MARKETING"; // or "UTILITY" or "AUTHENTICATION"
            var request = new BaseCreateTemplateMessageRequest
            {
                Name = $"test_template_{DateTime.UtcNow:yyyyMMddHHmmss}",
                Category = templateCategory,
                Language = "en_US",
                Components = new List<object>
                {
                    new TemplateComponent
                    {
                        Type = "HEADER",
                        Format = "TEXT",
                        Text = $"{{{{{companyNameParameterName}}}}} Intro",
                        Example = new TemplateComponentParameterExample
                        {
                            HeaderTextNamedParameters = new List<TemplateComponentNamedParameter>
                            {
                                new TemplateComponentNamedParameter
                                {
                                    Name = companyNameParameterName,
                                    Example = companyNameExampleText
                                }
                            }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "BODY",
                        Text = $"Hey {{{{{firstNameParameterName}}}}}! {{{{{companyNameParameterName}}}}} would love for you to schedule an appointment with us.",
                        Example = new TemplateComponentParameterExample
                        {
                            BodyTextNamedParameters = new List<TemplateComponentNamedParameter>
                            {
                                new TemplateComponentNamedParameter
                                {
                                    Name = firstNameParameterName,
                                    Example = firstNameExampleText
                                },
                                new TemplateComponentNamedParameter
                                {
                                    Name = companyNameParameterName,
                                    Example = companyNameExampleText
                                }
                            }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "FOOTER",
                        Text = "Reply STOP to unsubscribe."
                    },
                    new TemplateComponent
                    {
                        Type = "BUTTONS",
                        Buttons = new List<TemplateComponentButton>
                        {
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "YES"
                            },
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "NO"
                            }
                        }
                    }
                }
            };

            // Act
            var response = await _client.CreateTemplateMessageAsync(_whatsAppConfig.WhatsAppBusinessAccountId, request);

            // Assert
            response.Status.Should().NotBeNullOrEmpty();
            response.Status.Should().BeOneOf("PENDING", "APPROVED");
            response.Id.Should().NotBeNullOrEmpty();
            response.Category.Should().Be(templateCategory);
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetAllTemplatesAsync_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAllTemplatesAsync(_whatsAppConfig.WhatsAppBusinessAccountId);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.Should().AllSatisfy(x => x.Id.Should().NotBeNullOrEmpty());
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetTemplateByIdAsync_ShouldReturnSuccess()
        {
            // Arrange
            var templateId = "your_template_id";
            // Act
            var response = await _client.GetTemplateByIdAsync(templateId);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().NotBeNullOrEmpty();
            response.Name.Should().NotBeNullOrEmpty();
            response.Status.Should().NotBeNullOrEmpty();
            response.Components.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetTemplateByNameAsync_ShouldReturnSuccess()
        {
            // Arrange
            var templateName = "hello_world";
            // Act
            var response = await _client.GetTemplateByNameAsync(_whatsAppConfig.WhatsAppBusinessAccountId, templateName);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNullOrEmpty();
            var templateData = response.Data.FirstOrDefault();
            templateData.Should().NotBeNull();
            templateData.Id.Should().NotBeNullOrEmpty();
            templateData.Name.Should().NotBeNullOrEmpty();
            templateData.Status.Should().NotBeNullOrEmpty();
            templateData.Components.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task DeleteTemplateByIdAsync_ShouldReturnSuccess()
        {
            // Arrange
            var templateId = "your_template_id";
            var templateName = "your_template_name";
            // Act
            var response = await _client.DeleteTemplateByIdAsync(_whatsAppConfig.WhatsAppBusinessAccountId, templateId, templateName);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task SendTextMessageTemplateAsync_WithoutParameters_ShouldReturnSuccess()
        {
            // Arrange
            var request = new TextTemplateMessageRequest
            {
                To = "your_registered_phone_number",
                Template = new TextMessageTemplate
                {
                    Name = "hello_world",
                    Language = new TextMessageLanguage
                    {
                        Code = "en_US"
                    }
                }
            };
            // Act
            var response = await _client.SendTextMessageTemplateAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNullOrEmpty();
            response.Contacts.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task SendTextMessageTemplateAsync_WithParameters_ShouldReturnSuccess()
        {
            // Arrange
            var phoneNumber = "your_phone_number"; // Example phone number, should be a valid WhatsApp registered number
            var templateName = "test_template_20250715112934"; // Example template name, should match an existing template
            var firstName = "John";
            var companyName = "My Company";

            var request = new TextTemplateMessageRequest
            {
                To = phoneNumber,
                Template = new TextMessageTemplate
                {
                    Name = templateName,
                    Language = new TextMessageLanguage
                    {
                        Code = "en_US"
                    },
                    Components = new List<TextMessageComponent>
                    {
                        new TextMessageComponent
                        {
                            Type = "HEADER",
                            Parameters = new List<TextMessageParameter>
                            {
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = companyName
                                }
                            }
                        },
                        new TextMessageComponent
                        {
                            Type = "BODY",
                            Parameters = new List<TextMessageParameter>
                            {
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = firstName
                                },
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = companyName
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var response = await _client.SendTextMessageTemplateAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNullOrEmpty();
            response.Contacts.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the EmbeddedSignupConfiguration to run the test.")]
        public async Task ExchangeTokenAsync_WithValidAuthorizationCode_ShouldReturnAccessToken()
        {
            // Arrange
            var authorizationCode = "your_authorization_code"; // Replace with actual authorization code obtained from the OAuth flow
            var clientId = "your_meta_app_id"; // Replace with actual Meta App ID from configuration
            var clientSecret = "your_meta_app_secret"; // Replace with actual Meta App Secret from configuration
            var redirectUri = "your_redirect_uri"; // Replace with actual redirect URI

            var exchangeRequest = new ExchangeTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Code = authorizationCode,
                RedirectUri = redirectUri
            };

            // Act
            var response = await _client.ExchangeTokenAsync(exchangeRequest);

            // Assert
            response.Should().NotBeNull();
            response.AccessToken.Should().NotBeNullOrEmpty();
            response.TokenType.Should().Be("bearer");
            response.ExpiresIn.Should().BeGreaterThan(0);
            response.Error.Should().BeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetSharedWABAIdAsync_ShouldReturnData()
        {
            // Arrange
            var inputToken = "business_token_obtained_after_embedded_signup";
            // Act
            var response = await _client.GetSharedWABAIdAsync(inputToken);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            var sharedWABAId = response.GetSharedWABAId();
            sharedWABAId.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetWhatsAppBusinessAccountPhoneNumberAsync_ShouldReturnData()
        {
            // Arrange
            var whatsAppBusinessAccountId = "your_waba_id";
            // Act
            var response = await _client.GetWhatsAppBusinessAccountPhoneNumberAsync(whatsAppBusinessAccountId);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            var lastOnboardedPhoneNumberId = response.GetMostRecentlyOnboardedPhoneNumberId();
            lastOnboardedPhoneNumberId.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task CreateTemplateMessageAsync_InSharedWaba_UsingPositionalParameters_ShouldReturnCorrectStatus()
        {
            // Arrange
            var firstNameExampleText = "John";
            var companyNameExampleText = "My Company";
            var templateCategory = "MARKETING"; // or "UTILITY" or "AUTHENTICATION"
            var sharedWabaId = _sharedWhatsAppConfigs.First().WhatsAppBusinessAccountId;
            var sharedWhatsAppClient = _sharedClients.First();

            var request = new BaseCreateTemplateMessageRequest
            {
                Name = $"test_template",
                Category = templateCategory,
                Language = "en_US",
                Components = new List<object>
                {
                    new TemplateComponent
                    {
                        Type = "HEADER",
                        Format = "TEXT",
                        Text = "{{1}} Intro",
                        Example = new TemplateComponentParameterExample
                        {
                            HeaderText = new List<string> { companyNameExampleText }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "BODY",
                        Text = "Hey {{1}}! {{2}} would love for you to schedule an appointment with us.",
                        Example = new TemplateComponentParameterExample
                        {
                            BodyText = new List<List<string>>
                            {
                                new List<string>{ firstNameExampleText, companyNameExampleText }
                            }
                        }
                    },
                    new TemplateComponent
                    {
                        Type = "FOOTER",
                        Text = "Reply STOP to unsubscribe."
                    },
                    new TemplateComponent
                    {
                        Type = "BUTTONS",
                        Buttons = new List<TemplateComponentButton>
                        {
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "YES"
                            },
                            new TemplateComponentButton
                            {
                                Type = "QUICK_REPLY",
                                Text = "NO"
                            }
                        }
                    }
                }
            };

            // Act
            var response = await sharedWhatsAppClient.CreateTemplateMessageAsync(sharedWabaId, request);

            // Assert
            response.Status.Should().NotBeNullOrEmpty();
            response.Status.Should().BeOneOf("PENDING", "APPROVED");
            response.Id.Should().NotBeNullOrEmpty();
            response.Category.Should().Be(templateCategory);
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task SendTextMessageTemplateAsync_FromSharedWaba_WithParameters_ShouldReturnSuccess()
        {
            // Arrange
            var phoneNumber = "phone_number_to_contact_from_shared_waba"; // Example phone number, should be a valid WhatsApp registered number
            var templateName = "test_template"; // Example template name, should match an existing template
            var firstName = "John";
            var companyName = "My Company";
            var sharedWhatsAppClient = _sharedClients.First();

            var request = new TextTemplateMessageRequest
            {
                To = phoneNumber,
                Template = new TextMessageTemplate
                {
                    Name = templateName,
                    Language = new TextMessageLanguage
                    {
                        Code = "en_US"
                    },
                    Components = new List<TextMessageComponent>
                    {
                        new TextMessageComponent
                        {
                            Type = "HEADER",
                            Parameters = new List<TextMessageParameter>
                            {
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = companyName
                                }
                            }
                        },
                        new TextMessageComponent
                        {
                            Type = "BODY",
                            Parameters = new List<TextMessageParameter>
                            {
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = firstName
                                },
                                new TextMessageParameter
                                {
                                    Type = "text",
                                    Text = companyName
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var response = await sharedWhatsAppClient.SendTextMessageTemplateAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNullOrEmpty();
            response.Contacts.Should().NotBeNullOrEmpty();
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetWABADetails_ShouldReturnSuccess()
        {
            // Arrange
            var wabaId = _whatsAppConfig.WhatsAppBusinessAccountId;

            // Act
            var response = await _client.GetWABADetailsAsync(wabaId);

            // Assert
            response.Should().NotBeNull();
            response.Name.Should().NotBeNullOrEmpty();
            response.BusinessVerificationStatus.Should().NotBeNullOrEmpty();
        }
    }
}