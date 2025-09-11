using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.NetFramework.WhatsApp;
using WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Configuration;
using Xunit;

namespace WhatsappBusiness.CloudApi.NetFramework.Tests
{
    public class WhatsAppBusinessCloudApiClientTests : IClassFixture<TestSetup>
    {
        private readonly WhatsAppBusinessCloudApiConfig _config;
        private readonly string _primaryWABAId;
        private readonly string _sharedWABAId;
        private readonly string _inputTokenForSharedWABA;

        public WhatsAppBusinessCloudApiClientTests(TestSetup testSetup)
        {
            var configuration = testSetup.ServiceProvider.GetRequiredService<IConfiguration>();

            _config = new WhatsAppBusinessCloudApiConfig();
            _config.WhatsAppBusinessId = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppBusinessId"];
            _config.WhatsAppAccessToken = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppAccessToken"];
            _config.WhatsAppGraphApiVersion = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppGraphApiVersion"];
            _config.WhatsAppEmbeddedSignupMetaAppId = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppEmbeddedSignupMetaAppId"];
            _config.WhatsAppEmbeddedSignupMetaAppSecret = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppEmbeddedSignupMetaAppSecret"];
            _config.WhatsAppEmbeddedSignupMetaConfigurationId = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppEmbeddedSignupMetaConfigurationId"];
            _config.WhatsAppEmbeddedSignupPartnerSolutionId = configuration.GetSection("WhatsAppBusinessCloudApiNetFrameworkConfiguration")["WhatsAppEmbeddedSignupPartnerSolutionId"];

            _primaryWABAId = configuration.GetSection("TestWABAIds")["PrimaryWABAId"];
            _sharedWABAId = configuration.GetSection("TestWABAIds")["SharedWABAId"];
            _inputTokenForSharedWABA = configuration.GetSection("TestTokens")["InputTokenForSharedWABA"];
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task ExchangeTokenAsync_WithValidAuthorizationCode_ShouldReturnAccessToken()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                var authorizationCode = "your_authorization_code"; // Replace with actual authorization code obtained from the OAuth flow
                var redirectUri = "your_redirect_uri"; // Replace with actual redirect URI

                // Act
                var response = await client.ExchangeTokenAsync(authorizationCode, redirectUri);

                // Assert
                response.Should().NotBeNull();
                response.AccessToken.Should().NotBeNullOrEmpty();
                response.TokenType.Should().Be("bearer");
                response.ExpiresIn.Should().BeGreaterThan(0);
                response.Error.Should().BeNullOrEmpty();
            }
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetSharedWABAIdAsync_ShouldReturnData()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act
                var response = await client.GetSharedWABAIdAsync(_inputTokenForSharedWABA);

                // Assert
                response.Should().NotBeNull();
                response.Data.Should().NotBeNull();
                var sharedWABAId = response.GetSharedWABAId();
                sharedWABAId.Should().NotBeNullOrEmpty();
                sharedWABAId.Should().Be(_sharedWABAId);
            }
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetWABADetailsAsync_ShouldReturnSuccess()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act
                var response = await client.GetWABADetailsAsync(_sharedWABAId);

                // Assert
                response.Should().NotBeNull();
                response.Id.Should().Be(_sharedWABAId);
                response.Name.Should().NotBeNullOrEmpty();
                response.BusinessVerificationStatus.Should().NotBeNullOrEmpty();
                response.HealthStatus.Should().NotBeNull();
                response.HealthStatus.Entities.Should().NotBeNullOrEmpty();
            }
        }

        [Fact(Skip = "Complete the WhatsAppBusinessCloudApiConfig to run the test.")]
        public async Task GetWhatsAppBusinessAccountPhoneNumberAsync_ShouldReturnData()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act
                var response = await client.GetWhatsAppBusinessAccountPhoneNumberAsync(_sharedWABAId);

                // Assert
                response.Should().NotBeNull();
                response.Data.Should().NotBeNull();
                response.Data.Should().NotBeEmpty();

                var phoneNumber = response.Data[0];
                phoneNumber.Id.Should().NotBeNullOrEmpty();
                phoneNumber.DisplayPhoneNumber.Should().NotBeNullOrEmpty();
                phoneNumber.VerifiedName.Should().NotBeNullOrEmpty();

                var lastOnboardedPhoneNumberId = response.GetMostRecentlyOnboardedPhoneNumberId();
                lastOnboardedPhoneNumberId.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public void WhatsAppBusinessCloudApiClient_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Assert
                client.Should().NotBeNull();
            }
        }

        [Fact]
        public void WhatsAppBusinessCloudApiClient_MultipleInstances_ShouldUseSameHttpClient()
        {
            // Arrange & Act
            using (var client1 = new WhatsAppBusinessCloudApiClient(_config))
            using (var client2 = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Assert
                client1.Should().NotBeNull();
                client2.Should().NotBeNull();
            }
        }

        [Fact]
        public void WhatsAppBusinessCloudApiConfig_ShouldInitializeWithCorrectValues()
        {
            // Assert
            _config.Should().NotBeNull();
            _config.WhatsAppBusinessId.Should().NotBeNullOrEmpty();
            _config.WhatsAppAccessToken.Should().NotBeNullOrEmpty();
            _config.WhatsAppGraphApiVersion.Should().NotBeNullOrEmpty();
            _config.WhatsAppEmbeddedSignupMetaAppId.Should().NotBeNullOrEmpty();
            _config.WhatsAppEmbeddedSignupMetaAppSecret.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExchangeTokenAsync_WithNullCode_ShouldThrowArgumentException()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => client.ExchangeTokenAsync(null));
            }
        }

        [Fact]
        public async Task ExchangeTokenAsync_WithEmptyCode_ShouldThrowArgumentException()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => client.ExchangeTokenAsync(""));
            }
        }

        [Fact]
        public async Task GetSharedWABAIdAsync_WithNullToken_ShouldThrowArgumentException()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => client.GetSharedWABAIdAsync(null));
            }
        }

        [Fact]
        public async Task GetWABADetailsAsync_WithNullWABAId_ShouldThrowArgumentException()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => client.GetWABADetailsAsync(null));
            }
        }

        [Fact]
        public async Task GetWhatsAppBusinessAccountPhoneNumberAsync_WithNullWABAId_ShouldThrowArgumentException()
        {
            // Arrange
            using (var client = new WhatsAppBusinessCloudApiClient(_config))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => client.GetWhatsAppBusinessAccountPhoneNumberAsync(null));
            }
        }
    }
}