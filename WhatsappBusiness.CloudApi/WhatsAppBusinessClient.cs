using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
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
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;
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
        private WhatsAppBusinessCloudApiConfig _whatsAppConfig;

        /// <summary>
        /// Initialize WhatsAppBusinessClient with httpclient factory
        /// </summary>
        /// <param name="whatsAppConfig">WhatsAppBusiness configuration</param>
        public WhatsAppBusinessClient(WhatsAppBusinessCloudApiConfig whatsAppConfig)
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
                client.BaseAddress = WhatsAppBusinessRequestEndpoint.BaseAddress;
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

        public BaseSuccessResponse ConfigureConversationalCommands(ConversationalComponentCommand conversationalComponentCommand, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?commands={JsonConvert.SerializeObject(conversationalComponentCommand)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> ConfigureConversationalCommandsAsync(ConversationalComponentCommand conversationalComponentCommand, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?commands={JsonConvert.SerializeObject(conversationalComponentCommand)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse ConfigureConversationalComponentPrompt(List<string> prompts, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?prompts={JsonConvert.SerializeObject(prompts)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> ConfigureConversationalComponentPromptAsync(List<string> prompts, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?prompts={JsonConvert.SerializeObject(prompts)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse CreateConversationalMessage(ConversationalComponentRequest conversationalComponentRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(conversationalComponentRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> CreateConversationalMessageAsync(ConversationalComponentRequest conversationalComponentRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(conversationalComponentRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="qrImageFormat"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public QRCodeMessageResponse CreateQRCodeMessage(string messageText, string qrImageFormat, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.CreateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{message-text}}", messageText);
            builder.Replace("{{image-format}}", qrImageFormat);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<QRCodeMessageResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="qrImageFormat"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public async Task<QRCodeMessageResponse> CreateQRCodeMessageAsync(string messageText, string qrImageFormat, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.CreateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{message-text}}", messageText);
            builder.Replace("{{image-format}}", qrImageFormat);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<QRCodeMessageResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
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
        public BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Create Whatsapp template message
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="template">Message template type</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Template Message Creation Response</returns>
		public async Task<TemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			return await WhatsAppBusinessPostAsync<TemplateMessageCreationResponse>(template, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Create Whatsapp template message
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="template">Message template type</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Template Message Creation Response</returns>
		public TemplateMessageCreationResponse CreateTemplateMessage(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			return WhatsAppBusinessPostAsync<TemplateMessageCreationResponse>(template, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// To delete media, make a DELETE call to the ID of the media you want to delete.
		/// </summary>
		/// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
		/// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse DeleteMedia(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            string formattedWhatsAppEndpoint;
            if (isMediaOwnershipVerified)
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMediaOwnership.Replace("{{Media-ID}}", mediaId).Replace("{{PHONE_NUMBER_ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            }
            else
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            }
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To delete media, make a DELETE call to the ID of the media you want to delete.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            string formattedWhatsAppEndpoint;
            if (isMediaOwnershipVerified)
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMediaOwnership.Replace("{{Media-ID}}", mediaId).Replace("{{PHONE_NUMBER_ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            }
            else
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            }
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// QR codes do not expire. You must delete a QR code in order to retire it.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse DeleteQRCodeMessage(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.DeleteQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// QR codes do not expire. You must delete a QR code in order to retire it.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeleteQRCodeMessageAsync(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.DeleteQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
        }

		/// <summary>
		/// Delete Message Template By Template Name
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public async Task<BaseSuccessResponse> DeleteTemplateByNameAsync(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.GetTemplateByName);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			builder.Replace("{{TEMPLATE_NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Delete Message Template By Template Name
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse DeleteTemplateByName(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.GetTemplateByName);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			builder.Replace("{{TEMPLATE_NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Delete Message Template by Template Id
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateId">Template Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public async Task<BaseSuccessResponse> DeleteTemplateByIdAsync(string whatsAppBusinessAccountId, string templateId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.DeleteTemplateMessage);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            builder.Replace("{{HSM_ID}}", templateId);
			builder.Replace("{{NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Delete Message Template by Template Id
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateId">Template Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse DeleteTemplatebyId(string whatsAppBusinessAccountId, string templateId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.DeleteTemplateMessage);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			builder.Replace("{{HSM_ID}}", templateId);
			builder.Replace("{{NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To download media uploaded from whatsapp
        /// </summary>
        /// <param name="mediaUrl">The URL generated from whatsapp cloud api</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>byte[]</returns>
        public byte[] DownloadMedia(string mediaUrl, string appName = null, string version = null, CancellationToken cancellationToken = default)
        {
            string formattedWhatsAppEndpoint;
            formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DownloadMedia.Replace("{{Media-URL}}", mediaUrl);
            return WhatsAppBusinessGetAsync(formattedWhatsAppEndpoint, appName, version, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To download media uploaded from whatsapp
        /// </summary>
        /// <param name="mediaUrl">The URL generated from whatsapp cloud api</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>byte[]</returns>
        public async Task<byte[]> DownloadMediaAsync(string mediaUrl, string appName = null, string version = null, CancellationToken cancellationToken = default)
        {
            string formattedWhatsAppEndpoint;
            formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DownloadMedia.Replace("{{Media-URL}}", mediaUrl);
            return await WhatsAppBusinessGetAsync(formattedWhatsAppEndpoint, appName, version, cancellationToken);
        }

		/// <summary>
		/// Edit Message Template for update
		/// </summary>
		/// <param name="messageTemplate">MessageTemplate Object</param>
		/// <param name="templateId">Template Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public async Task<BaseSuccessResponse> EditTemplateAsync(object messageTemplate, string templateId, CancellationToken cancellationToken = default)
        {
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateById.Replace("{{TEMPLATE_ID}}", templateId);
			return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(messageTemplate, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Edit Message Template for update
		/// </summary>
		/// <param name="messageTemplate">MessageTemplate Object</param>
		/// <param name="templateId">Template Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse EditTemplate(object messageTemplate, string templateId, CancellationToken cancellationToken = default)
        {
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateById.Replace("{{TEMPLATE_ID}}", templateId);
			return WhatsAppBusinessPostAsync<BaseSuccessResponse>(messageTemplate, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

        public BaseSuccessResponse EnableConversationalWelcomeMessage(bool isWelcomeMessageEnabled, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?enable_welcome_message={isWelcomeMessageEnabled}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> EnableConversationalWelcomeMessageAsync(bool isWelcomeMessageEnabled, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?enable_welcome_message={isWelcomeMessageEnabled}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// The analytics field provides the number and type of messages sent and delivered by the phone numbers associated with a specific WABA
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="startDate">The start date for the date range you are retrieving analytics for</param>
        /// <param name="endDate">The end date for the date range you are retrieving analytics for</param>
        /// <param name="granularity">The granularity by which you would like to retrieve the analytics</param>
        /// <param name="phoneNumbers">An array of phone numbers for which you would like to retrieve analytics. If not provided, all phone numbers added to your WABA are included.</param>
        /// <param name="productTypes">The types of messages (notification messages and/or customer support messages) for which you want to retrieve notifications. Provide an array and include 0 for notification messages, and 2 for customer support messages. If not provided, analytics will be returned for all messages together</param>
        /// <param name="countryCodes">The countries for which you would like to retrieve analytics. Provide an array with 2 letter country codes for the countries you would like to include. If not provided, analytics will be returned for all countries you have communicated with</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>AnalyticsResponse</returns>
        public AnalyticsResponse GetAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            StringBuilder analyticUrlBuilder = new StringBuilder();
            analyticUrlBuilder.Append(WhatsAppBusinessRequestEndpoint.AnalyticsAccountMetrics);

            string formattedWhatsAppEndpoint;

            if (phoneNumbers is null)
            {
                phoneNumbers = new();
            }

            if (productTypes is null)
            {
                productTypes = new();
            }

            if (countryCodes is null)
            {
                countryCodes = new();
            }

            analyticUrlBuilder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            analyticUrlBuilder.Replace("{{start-date}}", new DateTimeOffset(startDate).ToUnixTimeSeconds().ToString());
            analyticUrlBuilder.Replace("{{end-date}}", new DateTimeOffset(endDate).ToUnixTimeSeconds().ToString());
            analyticUrlBuilder.Replace("{{granularity}}", granularity);
            analyticUrlBuilder.Append($".phone_numbers({JsonConvert.SerializeObject(phoneNumbers)})");
            analyticUrlBuilder.Append($".product_types({JsonConvert.SerializeObject(productTypes)})");
            analyticUrlBuilder.Append($".country_codes({JsonConvert.SerializeObject(countryCodes)})");
			analyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

			formattedWhatsAppEndpoint = analyticUrlBuilder.ToString();

            return WhatsAppBusinessGetAsync<AnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The analytics field provides the number and type of messages sent and delivered by the phone numbers associated with a specific WABA
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="startDate">The start date for the date range you are retrieving analytics for</param>
        /// <param name="endDate">The end date for the date range you are retrieving analytics for</param>
        /// <param name="granularity">The granularity by which you would like to retrieve the analytics</param>
        /// <param name="phoneNumbers">An array of phone numbers for which you would like to retrieve analytics. If not provided, all phone numbers added to your WABA are included.</param>
        /// <param name="productTypes">The types of messages (notification messages and/or customer support messages) for which you want to retrieve notifications. Provide an array and include 0 for notification messages, and 2 for customer support messages. If not provided, analytics will be returned for all messages together</param>
        /// <param name="countryCodes">The countries for which you would like to retrieve analytics. Provide an array with 2 letter country codes for the countries you would like to include. If not provided, analytics will be returned for all countries you have communicated with</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>AnalyticsResponse</returns>
        public async Task<AnalyticsResponse> GetAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            StringBuilder analyticUrlBuilder = new StringBuilder();
            analyticUrlBuilder.Append(WhatsAppBusinessRequestEndpoint.AnalyticsAccountMetrics);

            string formattedWhatsAppEndpoint;

            if (phoneNumbers is null)
            {
                phoneNumbers = new();
            }

            if (productTypes is null)
            {
                productTypes = new();
            }

            if (countryCodes is null)
            {
                countryCodes = new();
            }

            analyticUrlBuilder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            analyticUrlBuilder.Replace("{{start-date}}", new DateTimeOffset(startDate).ToUnixTimeSeconds().ToString());
            analyticUrlBuilder.Replace("{{end-date}}", new DateTimeOffset(endDate).ToUnixTimeSeconds().ToString());
            analyticUrlBuilder.Replace("{{granularity}}", granularity);
            analyticUrlBuilder.Append($".phone_numbers({JsonConvert.SerializeObject(phoneNumbers)})");
            analyticUrlBuilder.Append($".product_types({JsonConvert.SerializeObject(productTypes)})");
            analyticUrlBuilder.Append($".country_codes({JsonConvert.SerializeObject(countryCodes)})");
            analyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

            formattedWhatsAppEndpoint = analyticUrlBuilder.ToString();

            return await WhatsAppBusinessGetAsync<AnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
        }

        /// <summary>
        /// The conversation_analytics field provides cost and conversation information for a specific WABA.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="startDate">The start date for the date range you are retrieving analytics for</param>
        /// <param name="endDate">The end date for the date range you are retrieving analytics for</param>
        /// <param name="granularity">The granularity by which you would like to retrieve the analytics</param>
        /// <param name="phoneNumbers">An array of phone numbers for which you would like to retrieve analytics. If not provided, all phone numbers added to your WABA are included.</param>
        /// <param name="metricTypes">List of metrics you would like to receive. If you send an empty list, we return results for all metric types.</param>
        /// <param name="conversationTypes">List of conversation types. If you send an empty list, we return results for all conversation types.</param>
        /// <param name="conversationDirections">List of conversation directions. If you send an empty list, we return results for all conversation directions</param>
        /// <param name="dimensions">List of breakdowns you would like to apply to your metrics. If you send an empty list, we return results without any breakdowns.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ConversationAnalyticsResponse</returns>
        public ConversationAnalyticsResponse GetConversationAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            StringBuilder conversationAnalyticUrlBuilder = new StringBuilder();
            conversationAnalyticUrlBuilder.Append(WhatsAppBusinessRequestEndpoint.ConversationAnalyticsAccountMetrics);

            string formattedWhatsAppEndpoint;

            if (phoneNumbers is null)
            {
                phoneNumbers = new();
            }

            if (metricTypes is null)
            {
                metricTypes = new();
            }

            if (conversationTypes is null)
            {
                conversationTypes = new();
            }

            if (conversationDirections is null)
            {
                conversationDirections = new();
            }

            if (dimensions is null)
            {
                dimensions = new();
            }

            conversationAnalyticUrlBuilder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            conversationAnalyticUrlBuilder.Replace("{{start-date}}", new DateTimeOffset(startDate).ToUnixTimeSeconds().ToString());
            conversationAnalyticUrlBuilder.Replace("{{end-date}}", new DateTimeOffset(endDate).ToUnixTimeSeconds().ToString());
            conversationAnalyticUrlBuilder.Replace("{{granularity}}", granularity);
            conversationAnalyticUrlBuilder.Append($".phone_numbers({JsonConvert.SerializeObject(phoneNumbers)})");
            conversationAnalyticUrlBuilder.Append($".metric_types({JsonConvert.SerializeObject(metricTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_types({JsonConvert.SerializeObject(conversationTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_directions({JsonConvert.SerializeObject(conversationDirections)})");
            conversationAnalyticUrlBuilder.Append($".dimensions({JsonConvert.SerializeObject(dimensions)})");
			conversationAnalyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

			formattedWhatsAppEndpoint = conversationAnalyticUrlBuilder.ToString();

            return WhatsAppBusinessGetAsync<ConversationAnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The conversation_analytics field provides cost and conversation information for a specific WABA.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="startDate">The start date for the date range you are retrieving analytics for</param>
        /// <param name="endDate">The end date for the date range you are retrieving analytics for</param>
        /// <param name="granularity">The granularity by which you would like to retrieve the analytics</param>
        /// <param name="phoneNumbers">An array of phone numbers for which you would like to retrieve analytics. If not provided, all phone numbers added to your WABA are included.</param>
        /// <param name="metricTypes">List of metrics you would like to receive. If you send an empty list, we return results for all metric types.</param>
        /// <param name="conversationTypes">List of conversation types. If you send an empty list, we return results for all conversation types.</param>
        /// <param name="conversationDirections">List of conversation directions. If you send an empty list, we return results for all conversation directions</param>
        /// <param name="dimensions">List of breakdowns you would like to apply to your metrics. If you send an empty list, we return results without any breakdowns.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ConversationAnalyticsResponse</returns>
        public async Task<ConversationAnalyticsResponse> GetConversationAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            StringBuilder conversationAnalyticUrlBuilder = new StringBuilder();
            conversationAnalyticUrlBuilder.Append(WhatsAppBusinessRequestEndpoint.ConversationAnalyticsAccountMetrics);

            string formattedWhatsAppEndpoint;

            if (phoneNumbers is null)
            {
                phoneNumbers = new();
            }

            if (metricTypes is null)
            {
                metricTypes = new();
            }

            if (conversationTypes is null)
            {
                conversationTypes = new();
            }

            if (conversationDirections is null)
            {
                conversationDirections = new();
            }

            if (dimensions is null)
            {
                dimensions = new();
            }

            conversationAnalyticUrlBuilder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            conversationAnalyticUrlBuilder.Replace("{{start-date}}", new DateTimeOffset(startDate).ToUnixTimeSeconds().ToString());
            conversationAnalyticUrlBuilder.Replace("{{end-date}}", new DateTimeOffset(endDate).ToUnixTimeSeconds().ToString());
            conversationAnalyticUrlBuilder.Replace("{{granularity}}", granularity);
            conversationAnalyticUrlBuilder.Append($".phone_numbers({JsonConvert.SerializeObject(phoneNumbers)})");
            conversationAnalyticUrlBuilder.Append($".metric_types({JsonConvert.SerializeObject(metricTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_types({JsonConvert.SerializeObject(conversationTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_directions({JsonConvert.SerializeObject(conversationDirections)})");
            conversationAnalyticUrlBuilder.Append($".dimensions({JsonConvert.SerializeObject(dimensions)})");
			conversationAnalyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

			formattedWhatsAppEndpoint = conversationAnalyticUrlBuilder.ToString();

            return await WhatsAppBusinessGetAsync<ConversationAnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
        }

        public ConversationalComponentResponse GetConversationalMessage(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<ConversationalComponentResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<ConversationalComponentResponse> GetConversationalMessageAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<ConversationalComponentResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        public BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        public async Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        public MediaUrlResponse GetMediaUrl(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            string formattedWhatsAppEndpoint;
            if (isMediaOwnershipVerified)
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrlOwnership.Replace("{{Media-ID}}", mediaId).Replace("{{PHONE_NUMBER_ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            }
            else
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            }
            return WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        public async Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            string formattedWhatsAppEndpoint;
            if (isMediaOwnershipVerified)
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrlOwnership.Replace("{{Media-ID}}", mediaId).Replace("{{PHONE_NUMBER_ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            }
            else
            {
                formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            }
            return await WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To get a list of all the QR codes messages for a business
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        public QRCodeMessageFilterResponse GetQRCodeMessageList(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessGetAsync<QRCodeMessageFilterResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To get a list of all the QR codes messages for a business
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        public async Task<QRCodeMessageFilterResponse> GetQRCodeMessageListAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessGetAsync<QRCodeMessageFilterResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To get information about a specific QR code message
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        public QRCodeMessageFilterResponse GetQRCodeMessageById(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetQRCodeMessageById);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessGetAsync<QRCodeMessageFilterResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To get information about a specific QR code message
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        public async Task<QRCodeMessageFilterResponse> GetQRCodeMessageByIdAsync(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetQRCodeMessageById);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessGetAsync<QRCodeMessageFilterResponse>(formattedWhatsAppEndpoint, cancellationToken);
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
        public SharedWABAResponse GetSharedWABAList(string businessId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        public async Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return await WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Get Whatsapp template message by namespace
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateNamespaceResponse</returns>
		public async Task<TemplateNamespaceResponse> GetTemplateNamespaceAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateNamespace.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			return await WhatsAppBusinessGetAsync<TemplateNamespaceResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Get Whatsapp template message by namespace
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateNamespaceResponse</returns>
		public TemplateNamespaceResponse GetTemplateNamespace(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateNamespace.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			return WhatsAppBusinessGetAsync<TemplateNamespaceResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Get Whatsapp Template Message by Id
		/// </summary>
		/// <param name="templateId">Template Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateByIdResponse</returns>
		public async Task<TemplateByIdResponse> GetTemplateByIdAsync(string templateId, CancellationToken cancellationToken = default)
        {
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateById.Replace("{{TEMPLATE_ID}}", templateId);
			return await WhatsAppBusinessGetAsync<TemplateByIdResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Get Whatsapp Template Message by Id
		/// </summary>
		/// <param name="templateId">Template Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateByIdResponse</returns>
		public TemplateByIdResponse GetTemplateById(string templateId, CancellationToken cancellationToken = default)
        {
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateById.Replace("{{TEMPLATE_ID}}", templateId);
			return WhatsAppBusinessGetAsync<TemplateByIdResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Get Whatsapp Template Message by Name
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateByNameResponse</returns>
		public async Task<TemplateByNameResponse> GetTemplateByNameAsync(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.GetTemplateByName);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			builder.Replace("{{TEMPLATE_NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessGetAsync<TemplateByNameResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Get Whatsapp Template Message by Name
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateByNameResponse</returns>
		public TemplateByNameResponse GetTemplateByName(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.GetTemplateByName);
			builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			builder.Replace("{{TEMPLATE_NAME}}", templateName);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessGetAsync<TemplateByNameResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

        /// <summary>
        /// Get All templates for the whatsapp business account
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>TemplateResponse</returns>
        public async Task<TemplateResponse> GetAllTemplatesAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetAllTemplateMessage);
            builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessGetAsync<TemplateResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Get All templates for the whatsapp business account
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>TemplateResponse</returns>
        public TemplateResponse GetAllTemplates(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetAllTemplateMessage);
            builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessGetAsync<TemplateResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        public SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        public async Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        public PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        public PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        public async Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        public async Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>MarkMessageResponse</returns>
		public MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

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
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>MarkMessageResponse</returns>
		public async Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

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
        public BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public async Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To register your phone, make a POST call to /{{Phone-Number-ID}}/register
        /// </summary>
        /// <param name="registerPhoneRequest">RegisterPhoneRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        public async Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public async Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Template Message
        /// </summary>
        /// <param name="documentTemplateMessageRequest">DocumentTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendDocumentAttachmentTemplateMessageAsync(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Document Template Message
        /// </summary>
        /// <param name="documentTemplateMessageRequest">DocumentTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendDocumentAttachmentTemplateMessage(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive CTA Button Message
        /// </summary>
        /// <param name="interactiveCTAButtonMessageRequest">InteractiveCTAButtonMessageRequest Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveCTAButtonMessage(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveCTAButtonMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive CTA Button Message
        /// </summary>
        /// <param name="interactiveCTAButtonMessageRequest">InteractiveCTAButtonMessageRequest Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveCTAButtonMessageAsync(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveCTAButtonMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send location template message
        /// </summary>
        /// <param name="locationTemplateMessageRequest">locationTemplateMessageRequesr object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendLocationTemplateMessage(LocationTemplateMessageRequest locationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(locationTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send location template message
        /// </summary>
        /// <param name="locationTemplateMessageRequest">locationTemplateMessageRequesr object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendLocationTemplateMessageAsync(LocationTemplateMessageRequest locationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(locationTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send reaction message reply
        /// </summary>
        /// <param name="reactionMessageReply">ReactionMessageReply Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendReactionMessageReply(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(reactionMessageReply, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send reaction message reply
        /// </summary>
        /// <param name="reactionMessageReply">ReactionMessageReply Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendReactionMessageReplyAsync(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(reactionMessageReply, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Video Template Message
        /// </summary>
        /// <param name="videoTemplateMessageRequest"></param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public WhatsAppResponse SendVideoAttachmentTemplateMessage(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Video Template Message
        /// </summary>
        /// <param name="videoTemplateMessageRequest">VideoTemplateMessageRequest Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendVideoAttachmentTemplateMessageAsync(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendSingleProductMessageAsync(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendSingleProductMessage(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendMultipleProductMessageAsync(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendMultipleProductMessage(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

		/// <summary>
		/// Send Authentication Template Message
		/// </summary>
		/// <param name="authenticationTemplateMessageRequest">AuthenticationTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendAuthenticationMessageTemplateAsync(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(authenticationTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Authentication Template Message
		/// </summary>
		/// <param name="authenticationTemplateMessageRequest">AuthenticationTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendAuthenticationMessageTemplate(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(authenticationTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send MPM Template Message
		/// </summary>
		/// <param name="multiProductTemplateMessageRequest">MultiProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendMPMTemplateAsync(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send MPM Template Message
		/// </summary>
		/// <param name="multiProductTemplateMessageRequest">MultiProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendMPMTemplate(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Catalog Template Message
		/// </summary>
		/// <param name="catalogTemplateMessageRequest">CatalogTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendCatalogMessageTemplateAsync(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(catalogTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Catalog Template Message
		/// </summary>
		/// <param name="catalogTemplateMessageRequest">CatalogTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendCatalogMessageTemplate(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(catalogTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Catalog Message
		/// </summary>
		/// <param name="catalogMessageRequest">CatalogMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendCatalogMessageAsync(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(catalogMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Catalog Message
		/// </summary>
		/// <param name="catalogMessageRequest">CatalogMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendCatalogMessage(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(catalogMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Carousel Template Message
		/// </summary>
		/// <param name="carouselTemplateMessageRequest">CarouselTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WHatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendCarouselMessageTemplateAsync(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(carouselTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Carousel Template Message
		/// </summary>
		/// <param name="carouselTemplateMessageRequest">CarouselTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WHatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendCarouselMessageTemplate(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(carouselTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Coupon Code Template Message
		/// </summary>
		/// <param name="couponCodeTemplateMessageRequest">CouponCodeTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendCouponCodeMessageTemplateAsync(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(couponCodeTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Coupon Code Template Message
		/// </summary>
		/// <param name="couponCodeTemplateMessageRequest">CouponCodeTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendCouponCodeMessageTemplate(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(couponCodeTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Limited Time Offer Template Message
		/// </summary>
		/// <param name="limitedTimeOfferTemplateMessageRequest">LimitedTimeOfferTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendLimitedTimeOfferMessageTemplateAsync(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(limitedTimeOfferTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Limited Time Offer Template Message
		/// </summary>
		/// <param name="limitedTimeOfferTemplateMessageRequest">LimitedTimeOfferTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendLimitedTimeOfferMessageTemplate(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(limitedTimeOfferTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Whatsapp Flow Messages
		/// </summary>
		/// <param name="flowMessageRequest">FlowMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>FlowMessageResponse</returns>
		public async Task<FlowMessageResponse> SendFlowMessageAsync(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<FlowMessageResponse>(flowMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Whatsapp Flow Messages
		/// </summary>
		/// <param name="flowMessageRequest">FlowMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>FlowMessageResponse</returns>
		public FlowMessageResponse SendFlowMessage(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<FlowMessageResponse>(flowMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Whatsapp Flow Template Messages
		/// </summary>
		/// <param name="flowTemplateMessageRequest">FlowTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendFlowMessageTemplateAsync(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(flowTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Whatsapp Flow Template Messages
		/// </summary>
		/// <param name="flowTemplateMessageRequest">FlowTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendFlowMessageTemplate(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<FlowMessageResponse>(flowTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

        /// <summary>
        /// Send generic or universal whatsapp message type that are not implemented in the library
        /// </summary>
        /// <param name="whatsAppMessageRequest">whatsAppMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public async Task<WhatsAppResponse> SendGenericMessageAsync(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(whatsAppMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Send generic or universal whatsapp message type that are not implemented in the library
        /// </summary>
        /// <param name="whatsAppMessageRequest">whatsAppMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        public WhatsAppResponse SendGenericMessage(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(whatsAppMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

		/// <summary>
		/// Location request messages are free-form messages displaying body text and a send location button. When a WhatsApp user taps the button, a location sharing screen appears which the user can then use to share their location.
		/// </summary>
		/// <param name="interactiveLocationMessageRequest">interactiveLocationMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public async Task<WhatsAppResponse> SendLocationRequestMessageAsync(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveLocationMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Location request messages are free-form messages displaying body text and a send location button. When a WhatsApp user taps the button, a location sharing screen appears which the user can then use to share their location.
		/// </summary>
		/// <param name="interactiveLocationMessageRequest">interactiveLocationMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public WhatsAppResponse SendLocationRequestMessage(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveLocationMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
		/// You set up two-factor verification and register a phone number in the same API call.
		/// </summary>
		/// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public async Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        public async Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UpdateBusinessProfileId.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(updateBusinessProfile, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To update a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls/{qr-code-id} endpoint and include the parameter you wish to update.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="messageText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public QRCodeMessageResponse UpdateQRCodeMessage(string qrCodeId, string messageText, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.UpdateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);
            builder.Replace("{{new-message-text}}", messageText);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<QRCodeMessageResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To update a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls/{qr-code-id} endpoint and include the parameter you wish to update.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="messageText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public async Task<QRCodeMessageResponse> UpdateQRCodeMessageAsync(string qrCodeId, string messageText, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.UpdateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);
            builder.Replace("{{new-message-text}}", messageText);
            builder.Replace("{{user-access-token}}", _whatsAppConfig.AccessToken);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<QRCodeMessageResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
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
        public MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaRequest.File, uploadMediaRequest.Type, cancellationToken, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public async Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaRequest.File, uploadMediaRequest.Type, cancellationToken, true);
        }

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.VerifyCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<VerificationResponse>(verifyCodeRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        public async Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

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
        private async Task<T> WhatsAppBusinessPostAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default, bool isHeaderAccessTokenProvided = true) where T : new()
        {
            if (isHeaderAccessTokenProvided)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            }
            
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
            if (!isMediaUpload) // Resumable upload
            {
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _whatsAppConfig.AccessToken);
                _httpClient.DefaultRequestHeaders.Add("file_offset", "0");
            }
            else
            {
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
			}
            
            T result = new();
            cancellationToken.ThrowIfCancellationRequested();

            FileInfo file = new FileInfo(filePath);
            var uploaded_file = System.IO.File.ReadAllBytes(filePath);

            string boundary = $"----------{Guid.NewGuid():N}";
            var content = new MultipartFormDataContent(boundary);

			HttpResponseMessage? response;

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

				response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);
			}
            else // Resumable upload
            {
				ByteArrayContent mediaFileContent = new ByteArrayContent(uploaded_file);

                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = mediaFileContent;
                requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

				requestMessage.RequestUri = new Uri($"{_httpClient.BaseAddress}{whatsAppBusinessEndpoint}");

				response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
			}

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

        private async Task<byte[]> WhatsAppBusinessGetAsync(string whatsAppBusinessEndpoint, string AppName = null, string version = null, CancellationToken cancellationToken = default)
        {
            ProductInfoHeaderValue productValue;

            if (!string.IsNullOrWhiteSpace(AppName) && !string.IsNullOrWhiteSpace(version))
            {
                productValue = new ProductInfoHeaderValue(AppName, version);
            }
            else
            {
                productValue = new ProductInfoHeaderValue(_whatsAppConfig.AppName, _whatsAppConfig.Version);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);

#if NET5_0_OR_GREATER
            var bytesDownloaded = await _httpClient.GetByteArrayAsync(whatsAppBusinessEndpoint, cancellationToken).ConfigureAwait(false);
#endif

#if NETSTANDARD2_0_OR_GREATER
            var bytesDownloaded = await _httpClient.GetByteArrayAsync(whatsAppBusinessEndpoint).ConfigureAwait(false);
#endif

            return bytesDownloaded;
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
        private async Task<T> WhatsAppBusinessGetAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default, bool isCacheControlActive = false, bool isHeaderAccessTokenProvided = true) where T : new()
        {
            if (isHeaderAccessTokenProvided && !isCacheControlActive)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            }

            if (isCacheControlActive) // Resumable upload
            {
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _whatsAppConfig.AccessToken);
				_httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                };
			}

            T result = new();
            cancellationToken.ThrowIfCancellationRequested();

			HttpResponseMessage? response;

            if (!isCacheControlActive)
            {
				response = await _httpClient.GetAsync(whatsAppBusinessEndpoint, cancellationToken).ConfigureAwait(false);
			}
            else
            {
				HttpRequestMessage requestMessage = new HttpRequestMessage();
				requestMessage.Method = HttpMethod.Get;
				requestMessage.RequestUri = new Uri($"{_httpClient.BaseAddress}{whatsAppBusinessEndpoint}");

				response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
			}

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
        private async Task<T> WhatsAppBusinessDeleteAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default, bool isHeaderAccessTokenProvided = true) where T : new()
        {
            if (isHeaderAccessTokenProvided)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            }
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
