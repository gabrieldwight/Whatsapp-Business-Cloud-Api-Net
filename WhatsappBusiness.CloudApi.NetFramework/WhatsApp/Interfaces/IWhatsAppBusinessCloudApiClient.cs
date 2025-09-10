using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Requests;
using WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Responses;

namespace WhatsappBusiness.CloudApi.NetFramework.WhatsApp.Interfaces
{
    /// <summary>
    /// Interface for WhatsApp Business Cloud API client
    /// </summary>
    public interface IWhatsAppBusinessCloudApiClient
    {
        /// <summary>
        /// Exchanges an authorization code for an access token asynchronously.
        /// This is used as part of the OAuth 2.0 authorization code flow for Meta Embedded Signup.
        /// </summary>
        /// <param name="code">Authorization code received from OAuth flow</param>
        /// <param name="redirectUri">Redirect URI used in OAuth flow</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ExchangeTokenResponse containing the access token or error information</returns>
        Task<ExchangeTokenResponse> ExchangeTokenAsync(string code, string redirectUri = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Shared WhatsApp Business Account ID from input token
        /// </summary>
        /// <param name="inputToken">Input token obtained after embedded signup</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        Task<SharedWABAIDResponse> GetSharedWABAIdAsync(string inputToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get detailed WhatsApp Business Account information by WABA ID
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">WhatsApp Business Account ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WABADetailsResponse</returns>
        Task<WABADetailsResponse> GetWABADetailsAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all phone numbers in a WhatsApp Business Account
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">WhatsApp Business Account ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);
    }
}