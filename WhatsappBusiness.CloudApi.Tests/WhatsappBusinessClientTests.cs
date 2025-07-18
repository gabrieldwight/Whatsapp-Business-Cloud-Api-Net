using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Templates;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WhatsappBusiness.CloudApi.Tests
{
    public class WhatsappBusinessClientTests : IClassFixture<TestSetup>
    {
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        private readonly WhatsAppBusinessClient _client;

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
            
            var factory = new WhatsAppBusinessClientFactory();
            _client = factory.Create(_whatsAppConfig);
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
    }
}