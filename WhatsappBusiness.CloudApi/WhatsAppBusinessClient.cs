using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.AccountMigration.Requests;
using WhatsappBusiness.CloudApi.BusinessProfile.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.PhoneNumbers.Requests;
using WhatsappBusiness.CloudApi.Registration.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.TwoStepVerification.Requests;

namespace WhatsappBusiness.CloudApi
{
    /// <summary>
    /// WhatsAppBusinessClient class provide functions that are supported by the cloud api
    /// </summary>
    public class WhatsAppBusinessClient : IWhatsAppBusinessClient
    {
        private readonly HttpClient _httpClient;
        readonly Random jitterer = new Random();
        private readonly JsonSerializer _serializer = new JsonSerializer();
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;

        /// <summary>
        /// Initialize WhatsAppBusinessClient with httpclient factory
        /// </summary>
        /// <param name="whatsAppConfig">WhatsAppBusiness configuration</param>
        /// <param name="isLatestGraphApiVersion">Set True if you want use v14, false if you want to use v13</param>
        public WhatsAppBusinessClient(WhatsAppBusinessCloudApiConfig whatsAppConfig, bool isLatestGraphApiVersion = false)
        {
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            var services = new ServiceCollection();
            services.AddHttpClient("WhatsAppBusinessApiClient", client =>
            {
                client.BaseAddress = (isLatestGraphApiVersion) ? WhatsAppBusinessRequestEndpoint.V14BaseAddress : WhatsAppBusinessRequestEndpoint.BaseAddress;
                client.Timeout = TimeSpan.FromMinutes(10);
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            _httpClient = httpClientFactory.CreateClient("WhatsAppBusinessApiClient");
            _whatsAppConfig = whatsAppConfig;
        }

        /// <summary>
        /// Initialize WhatsAppBusinessClient with dependency injection
        /// </summary>
        /// <param name="httpClient">WhatsAppBusiness configuration</param>
        /// <param name="whatsAppConfig">Set True if you want use v14, false if you want to use v13</param>
        public WhatsAppBusinessClient(HttpClient httpClient, WhatsAppBusinessCloudApiConfig whatsAppConfig)
        {
            _httpClient = httpClient;
            _whatsAppConfig = whatsAppConfig;
        }

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        /// <param name="fileLength">File length</param>
        /// <param name="fileType">File Content type</param>
        /// <param name="fileName">Full Path of the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public async Task<ResumableUploadResponse> CreateResumableUploadSessionAsync(long fileLength, string fileType, string fileName, CancellationToken cancellationToken = default)
        {
            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.ResumableUploadCreateUploadSession);

            builder.Replace("{{FILE_LENGTH}}", fileLength.ToString());
            builder.Replace("{{FILE_TYPE}}", fileType);
            builder.Replace("{{FILE_NAME}}", fileName);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        /// <param name="fileLength">File length</param>
        /// <param name="fileType">File Content type</param>
        /// <param name="fileName">Full Path of the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public ResumableUploadResponse CreateResumableUploadSession(long fileLength, string fileType, string fileName, CancellationToken cancellationToken = default)
        {
            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.ResumableUploadCreateUploadSession);

            builder.Replace("{{FILE_LENGTH}}", fileLength.ToString());
            builder.Replace("{{FILE_TYPE}}", fileType);
            builder.Replace("{{FILE_NAME}}", fileName);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To delete media, make a DELETE call to the ID of the media you want to delete.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse DeleteMedia(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To delete media, make a DELETE call to the ID of the media you want to delete.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        public BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        public async Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        public MediaUrlResponse GetMediaUrl(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            return WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        public async Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            return await WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        public SharedWABAIDResponse GetSharedWABAId(CancellationToken cancellationToken = default)
        {
            return WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        public async Task<SharedWABAIDResponse> GetSharedWABAIdAsync(CancellationToken cancellationToken = default)
        {
            return await WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID, cancellationToken);
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        public SharedWABAResponse GetSharedWABAList(string businessId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        public async Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return await WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        public SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        public async Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        public PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        public PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        public async Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        public async Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// When you receive an incoming message from Webhooks, you could use messages endpoint to change the status of it to read.
        /// We recommend marking incoming messages as read within 30 days of receipt.
        /// Note: you cannot mark outgoing messages you sent as read.
        /// You need to obtain the message_id of the incoming message from Webhooks.
        /// </summary>
        /// <param name="markMessage">MarkMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MarkMessageResponse</returns>
        public MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MarkMessageAsRead.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MarkMessageResponse>(markMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// When you receive an incoming message from Webhooks, you could use messages endpoint to change the status of it to read.
        /// We recommend marking incoming messages as read within 30 days of receipt.
        /// Note: you cannot mark outgoing messages you sent as read.
        /// You need to obtain the message_id of the incoming message from Webhooks.
        /// </summary>
        /// <param name="markMessage">MarkMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MarkMessageResponse</returns>
        public async Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MarkMessageAsRead.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MarkMessageResponse>(markMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To migrate your account, make a POST call to the /{{Phone-Number-ID}}/register endpoint and include the parameters listed below.
        /// Your request may take as long as 15 seconds to finish.During this period, your on-premise deployment is automatically disconnected from WhatsApp server and shutdown; the business account will start up in the cloud-hosted service at the same time. After the request finishes successfully, you can send messages immediately.
        /// </summary>
        /// <param name="migrateAccountRequest">MigrateAccountRequest Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MigrateAccount.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(migrateAccountRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To migrate your account, make a POST call to the /{{Phone-Number-ID}}/register endpoint and include the parameters listed below.
        /// Your request may take as long as 15 seconds to finish.During this period, your on-premise deployment is automatically disconnected from WhatsApp server and shutdown; the business account will start up in the cloud-hosted service at the same time. After the request finishes successfully, you can send messages immediately.
        /// </summary>
        /// <param name="migrateAccountRequest">MigrateAccountRequest Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MigrateAccount.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(migrateAccountRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
        /// When uploading data, you must include the access token as an HTTP header.
        /// </summary>
        /// <param name="uploadId">Upload session</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public async Task<ResumableUploadResponse> QueryFileUploadStatusAsync(string uploadId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadQueryFileUploadStatus.Replace("{{Upload-ID}}", uploadId);
            return await WhatsAppBusinessGetAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, cancellationToken, true);
        }

        /// <summary>
        /// Query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
        /// When uploading data, you must include the access token as an HTTP header.
        /// </summary>
        /// <param name="uploadId">Upload session</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public ResumableUploadResponse QueryFileUploadStatus(string uploadId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadQueryFileUploadStatus.Replace("{{Upload-ID}}", uploadId);
            return WhatsAppBusinessGetAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, cancellationToken, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To register your phone, make a POST call to /{{Phone-Number-ID}}/register
        /// </summary>
        /// <param name="registerPhoneRequest">RegisterPhoneRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To register your phone, make a POST call to /{{Phone-Number-ID}}/register
        /// </summary>
        /// <param name="registerPhoneRequest">RegisterPhoneRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified below.
        /// To verify a phone number using Graph API, make a POST request to {{PHONE_NUMBER_ID
        /// }}/ request_code.In your call, include your chosen verification method and locale. You need to authenticate yourself using { { User - Access - Token} } (This is automatically done for you in the Request Verification Code request).
        /// </summary>
        /// <param name="requestVerification">RequestVerificationCode object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified below.
        /// To verify a phone number using Graph API, make a POST request to {{PHONE_NUMBER_ID
        /// }}/ request_code.In your call, include your chosen verification method and locale. You need to authenticate yourself using { { User - Access - Token} } (This is automatically done for you in the Request Verification Code request).
        /// </summary>
        /// <param name="requestVerification">RequestVerificationCode object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public async Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken);
        }
        
        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendSingleProductMessageAsync(SingleProductMessageRequest singleProductMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendSingleProductMessage(SingleProductMessageRequest singleProductMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendMultipleProductMessageAsync(MultiProductMessageRequest multiProductMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendMultipleProductMessage(MultiProductMessageRequest multiProductMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
        /// You set up two-factor verification and register a phone number in the same API call.
        /// </summary>
        /// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetTwoFactor.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(twoStepVerificationRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
        /// You set up two-factor verification and register a phone number in the same API call.
        /// </summary>
        /// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetTwoFactor.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(twoStepVerificationRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Update the business profile information such as the business description, email or address. To update your profile, make a POST call to /{{Phone-Number-ID}}/whatsapp_business_profile. In your request, you can include the parameters listed below.
        /// It is recommended that you use Resumable Upload - Create an Upload Session to obtain an upload ID.Then use this upload ID in a call to Resumable Upload - Upload File Data to obtain the picture handle.This handle can be used for the profile_picture_handle
        /// </summary>
        /// <param name="updateBusinessProfileRequest">UpdateBusinessProfileRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UpdateBusinessProfileId.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(updateBusinessProfileRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update the business profile information such as the business description, email or address. To update your profile, make a POST call to /{{Phone-Number-ID}}/whatsapp_business_profile. In your request, you can include the parameters listed below.
        /// It is recommended that you use Resumable Upload - Create an Upload Session to obtain an upload ID.Then use this upload ID in a call to Resumable Upload - Upload File Data to obtain the picture handle.This handle can be used for the profile_picture_handle
        /// </summary>
        /// <param name="updateBusinessProfile">UpdateBusinessProfileRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UpdateBusinessProfileId.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(updateBusinessProfile, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public async Task<ResumableUploadResponse> UploadFileDataAsync(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadFileData.Replace("{{Upload-ID}}", uploadId);
            return await WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, filePath, fileContentType, cancellationToken);
        }

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public ResumableUploadResponse UploadFileData(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadFileData.Replace("{{Upload-ID}}", uploadId);
            return WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, filePath, fileContentType, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaRequest.File, uploadMediaRequest.Type, cancellationToken, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public async Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaRequest.File, uploadMediaRequest.Type, cancellationToken, true);
        }

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.VerifyCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<VerificationResponse>(verifyCodeRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public async Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.VerifyCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(verifyCodeRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To perform WhatsApp Business Cloud API functions
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppDto">WhatsAppDto object</param>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business CLoud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response Object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPostAsync<T>(object whatsAppDto, string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            T result = new();
            string json = JsonConvert.SerializeObject(whatsAppDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }

        /// <summary>
        /// To perform core WhatsApp Business Account functions
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business Cloud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response Object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPostAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            T result = new();
            cancellationToken.ThrowIfCancellationRequested();

            var response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, null, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }

        /// <summary>
        /// To upload a profile picture to your business profile and media upload.
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business Media Upload endpoint</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPostAsync<T>(string whatsAppBusinessEndpoint, string filePath, string fileContentType, CancellationToken cancellationToken = default, bool isMediaUpload = false) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            
            if (!isMediaUpload) // Resumable upload
            {
                _httpClient.DefaultRequestHeaders.Add("Content-Type", fileContentType);
                _httpClient.DefaultRequestHeaders.Add("file_offset", "0");
            }
            
            T result = new();
            cancellationToken.ThrowIfCancellationRequested();

            FileInfo file = new FileInfo(filePath);
            var uploaded_file = System.IO.File.ReadAllBytes(filePath);

            string boundary = $"----------{Guid.NewGuid():N}";
            var content = new MultipartFormDataContent(boundary);

            if (isMediaUpload)
            {
                ByteArrayContent mediaFileContent = new ByteArrayContent(uploaded_file);
                mediaFileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.FullName,
                };
                mediaFileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileContentType);

                var fileData = new
                {
                    messaging_product = "whatsapp"
                };

                content.Add(mediaFileContent);
                content.Add(new StringContent(fileData.messaging_product), "messaging_product");
            }
            else
            {
                ByteArrayContent mediaFileContent = new ByteArrayContent(uploaded_file);
                mediaFileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.FullName,
                };
                content.Add(mediaFileContent);
            }

            var response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }

        /// <summary>
        /// To perform WhatsAppBusiness Cloud API endpoint GET request 
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business Cloud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="isCacheControlActive">Resumable upload header parameter</param>
        /// <returns>Response object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessGetAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default, bool isCacheControlActive = false) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            
            if (isCacheControlActive)
            {
                _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                };
            }

            T result = new();
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.GetAsync(whatsAppBusinessEndpoint, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }

        /// <summary>
        /// To perform WhatsAppBusiness Cloud API endpoint PUT request
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppDto">WhatsAppDto Object</param>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business Cloud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPutAsync<T>(object whatsAppDto, string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            T result = new();
            string json = JsonConvert.SerializeObject(whatsAppDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.PutAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }

        /// <summary>
        /// To perform WhatsAppBusiness Cloud API endpoint DELETE request
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppBusinessEndpoint">WhatsAPp Business CLoud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessDeleteAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            T result = new();
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.DeleteAsync(whatsAppBusinessEndpoint, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
#endif
            }
            else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
                await response.Content.ReadAsStreamAsync(cancellationToken).ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    whatsAppErrorResponse = _serializer.Deserialize<WhatsAppErrorResponse>(json);
                }, cancellationToken);
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
            }
            return result;
        }
    }
}
