﻿using Microsoft.Extensions.DependencyInjection;
#if !NET472
using Polly;
using Polly.Extensions.Http;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.AccountMigration.Requests;
using WhatsappBusiness.CloudApi.BlockUser.Requests;
using WhatsappBusiness.CloudApi.BusinessProfile.Requests;
using WhatsappBusiness.CloudApi.Calls.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Groups.Requests;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.MessageHistory.Requests;
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.OAuth.Requests;
using WhatsappBusiness.CloudApi.PhoneNumbers.Requests;
using WhatsappBusiness.CloudApi.Registration.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.Templates;
using WhatsappBusiness.CloudApi.TwoStepVerification.Requests;

namespace WhatsappBusiness.CloudApi
{
    /// <summary>
    /// WhatsAppBusinessClient class provide functions that are supported by the cloud api
    /// </summary>
    public class WhatsAppBusinessClient : IWhatsAppBusinessClient
    {
        protected readonly HttpClient _httpClient;
#if !NET472
        protected readonly Random _jitterer = new Random();
#endif
        protected WhatsAppBusinessCloudApiConfig _whatsAppConfig;

        /// <summary>
        /// Initialize WhatsAppBusinessClient with httpclient factory
        /// </summary>
        /// <param name="whatsAppConfig">WhatsAppBusiness configuration</param>
        public WhatsAppBusinessClient(WhatsAppBusinessCloudApiConfig whatsAppConfig, string? graphAPIVersion = null)
        {
#if !NET472
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(_jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
#endif

            var services = new ServiceCollection();
            services.AddHttpClient("WhatsAppBusinessApiClient", client =>
            {
                client.BaseAddress = (string.IsNullOrWhiteSpace(graphAPIVersion)) ? WhatsAppBusinessRequestEndpoint.BaseAddress : new Uri(WhatsAppBusinessRequestEndpoint.GraphApiVersionBaseAddress.ToString().Replace("{{api-version}}", graphAPIVersion));
                client.Timeout = TimeSpan.FromMinutes(10);
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
#if !NET472
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);
#else
            });
#endif

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            _httpClient = httpClientFactory.CreateClient("WhatsAppBusinessApiClient");
            _whatsAppConfig = whatsAppConfig;
        }

		/// <summary>
		/// Initialize WhatsAppBusinessClient with dependency injection
		/// </summary>
		/// <param name="httpClient">httpclient object</param>
		/// <param name="whatsAppConfig">WhatsAppBusiness configuration</param>
		public WhatsAppBusinessClient(HttpClient httpClient, WhatsAppBusinessCloudApiConfig whatsAppConfig)
        {
            _httpClient = httpClient;
            _whatsAppConfig = whatsAppConfig;
        }

        public virtual void SetWhatsAppBusinessConfig(WhatsAppBusinessCloudApiConfig cloudApiConfig)
        {
            _whatsAppConfig = cloudApiConfig;
		}

		public virtual async Task<WhatsAppGroupJoinRequestResponse> ApproveJoinRequestsAsync(GroupJoinRequest groupJoinRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
			return await WhatsAppBusinessPostAsync<WhatsAppGroupJoinRequestResponse>(groupJoinRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public virtual WhatsAppGroupJoinRequestResponse ApproveJoinRequests(GroupJoinRequest groupJoinRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessPostAsync<WhatsAppGroupJoinRequestResponse>(groupJoinRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Use this endpoint to block a list of WhatsApp user numbers.
		/// </summary>
		/// <param name="blockUserRequest">Block User Request</param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BlockUserResponse</returns>
		public virtual BlockUserResponse BlockUser(BlockUserRequest blockUserRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessPostAsync<BlockUserResponse>(blockUserRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Use this endpoint to block a list of WhatsApp user numbers.
		/// </summary>
		/// <param name="blockUserRequest">Block User Request</param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BlockUserResponse</returns>
		public virtual async Task<BlockUserResponse> BlockUserAsync(BlockUserRequest blockUserRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessPostAsync<BlockUserResponse>(blockUserRequest, formattedWhatsAppEndpoint, cancellationToken);
		}


		public virtual BaseSuccessResponse ConfigureConversationalCommands(ConversationalComponentCommand conversationalComponentCommand, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?commands={JsonSerializer.Serialize(conversationalComponentCommand)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public virtual async Task<BaseSuccessResponse> ConfigureConversationalCommandsAsync(ConversationalComponentCommand conversationalComponentCommand, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?commands={JsonSerializer.Serialize(conversationalComponentCommand)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public virtual BaseSuccessResponse ConfigureConversationalComponentPrompt(List<string> prompts, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?prompts={JsonSerializer.Serialize(prompts)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public virtual async Task<BaseSuccessResponse> ConfigureConversationalComponentPromptAsync(List<string> prompts, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.SetConversationAutomation);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Append($"?prompts={JsonSerializer.Serialize(prompts)}");

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public virtual BaseSuccessResponse CreateConversationalMessage(ConversationalComponentRequest conversationalComponentRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(conversationalComponentRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public virtual async Task<BaseSuccessResponse> CreateConversationalMessageAsync(ConversationalComponentRequest conversationalComponentRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(conversationalComponentRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupResponse> CreateGroupAsync(GroupRequest createGroupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Groups.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppGroupResponse>(createGroupRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse CreateGroup(GroupRequest createGroupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Groups.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppGroupResponse>(createGroupRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
		/// </summary>
		/// <param name="messageText"></param>
		/// <param name="qrImageFormat"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>QRCodeMessageResponse</returns>
		public virtual QRCodeMessageResponse CreateQRCodeMessage(string messageText, string qrImageFormat, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.CreateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();

			QRCodeMessageRequest qRCodeMessageRequest = new QRCodeMessageRequest
			{
				PrefilledMessage = messageText,
				GenerateQRImage = qrImageFormat
			};

			return WhatsAppBusinessPostAsync<QRCodeMessageResponse>(qRCodeMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="qrImageFormat"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public virtual async Task<QRCodeMessageResponse> CreateQRCodeMessageAsync(string messageText, string qrImageFormat, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.CreateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();

            QRCodeMessageRequest qRCodeMessageRequest = new QRCodeMessageRequest
			{
				PrefilledMessage = messageText,
				GenerateQRImage = qrImageFormat
			};

			return await WhatsAppBusinessPostAsync<QRCodeMessageResponse>(qRCodeMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        /// <param name="fileLength">File length</param>
        /// <param name="fileType">File Content type</param>
        /// <param name="fileName">Full Path of the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public virtual async Task<ResumableUploadResponse> CreateResumableUploadSessionAsync(long fileLength, string fileType, string fileName, CancellationToken cancellationToken = default)
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
        public virtual ResumableUploadResponse CreateResumableUploadSession(long fileLength, string fileType, string fileName, CancellationToken cancellationToken = default)
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
        public virtual BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<TemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<TemplateMessageCreationResponse> CreateTemplateMessageAsync(string whatsAppBusinessAccountId, BaseCreateTemplateMessageRequest template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual TemplateMessageCreationResponse CreateTemplateMessage(string whatsAppBusinessAccountId, object template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CreateTemplateMessage.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
			return WhatsAppBusinessPostAsync<TemplateMessageCreationResponse>(template, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

        /// <summary>
        /// Create Whatsapp template message
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
        /// <param name="template">Message template type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Template Message Creation Response</returns>
        public virtual TemplateMessageCreationResponse CreateTemplateMessage(string whatsAppBusinessAccountId, BaseCreateTemplateMessageRequest template, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual BaseSuccessResponse DeleteMedia(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
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
        public virtual BaseSuccessResponse DeleteQRCodeMessage(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.DeleteQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// QR codes do not expire. You must delete a QR code in order to retire it.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>BaseSuccessResponse</returns>
        public virtual async Task<BaseSuccessResponse> DeleteQRCodeMessageAsync(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.DeleteQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            builder.Replace("{{qr-code-id}}", qrCodeId);

            var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: true);
        }

		/// <summary>
		/// Delete Message Template By Template Name
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="templateName">Template Name</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual async Task<BaseSuccessResponse> DeleteTemplateByNameAsync(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual BaseSuccessResponse DeleteTemplateByName(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<BaseSuccessResponse> DeleteTemplateByIdAsync(string whatsAppBusinessAccountId, string templateId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual BaseSuccessResponse DeleteTemplatebyId(string whatsAppBusinessAccountId, string templateId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupResponse> DeleteWhatsAppGroupAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessDeleteAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse DeleteWhatsAppGroup(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessDeleteAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
		/// </summary>
		/// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual byte[] DownloadMedia(string mediaUrl, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			string formattedWhatsAppEndpoint;
            formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DownloadMedia.Replace("{{Media-URL}}", mediaUrl);
            return WhatsAppBusinessGetAsync(formattedWhatsAppEndpoint, _whatsAppConfig.AppName?.Replace(" ", "_"), _whatsAppConfig.Version, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To download media uploaded from whatsapp
        /// </summary>
        /// <param name="mediaUrl">The URL generated from whatsapp cloud api</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>byte[]</returns>
        public virtual async Task<byte[]> DownloadMediaAsync(string mediaUrl, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			string formattedWhatsAppEndpoint;
            formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DownloadMedia.Replace("{{Media-URL}}", mediaUrl);
            return await WhatsAppBusinessGetAsync(formattedWhatsAppEndpoint, _whatsAppConfig.AppName?.Replace(" ","_"), _whatsAppConfig.Version, cancellationToken);
        }

		/// <summary>
		/// Edit Message Template for update
		/// </summary>
		/// <param name="messageTemplate">MessageTemplate Object</param>
		/// <param name="templateId">Template Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual async Task<BaseSuccessResponse> EditTemplateAsync(object messageTemplate, string templateId, CancellationToken cancellationToken = default)
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
		public virtual BaseSuccessResponse EditTemplate(object messageTemplate, string templateId, CancellationToken cancellationToken = default)
        {
			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetTemplateById.Replace("{{TEMPLATE_ID}}", templateId);
			return WhatsAppBusinessPostAsync<BaseSuccessResponse>(messageTemplate, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

        public virtual BaseSuccessResponse EnableConversationalWelcomeMessage(bool isWelcomeMessageEnabled, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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

        public virtual async Task<BaseSuccessResponse> EnableConversationalWelcomeMessageAsync(bool isWelcomeMessageEnabled, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual AnalyticsResponse GetAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
            analyticUrlBuilder.Append($".phone_numbers({JsonSerializer.Serialize(phoneNumbers)})");
            analyticUrlBuilder.Append($".product_types({JsonSerializer.Serialize(productTypes)})");
            analyticUrlBuilder.Append($".country_codes({JsonSerializer.Serialize(countryCodes)})");
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
        public virtual async Task<AnalyticsResponse> GetAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
			analyticUrlBuilder.Append($".phone_numbers({JsonSerializer.Serialize(phoneNumbers)})");
			analyticUrlBuilder.Append($".product_types({JsonSerializer.Serialize(productTypes)})");
			analyticUrlBuilder.Append($".country_codes({JsonSerializer.Serialize(countryCodes)})");
			analyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

            formattedWhatsAppEndpoint = analyticUrlBuilder.ToString();

            return await WhatsAppBusinessGetAsync<AnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
        }

		/// <summary>
		/// Get a list of blocked users.
		/// </summary>
		/// <param name="limit"></param>
		/// <param name="after"></param>
		/// <param name="before"></param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>GetBlockedUserResponse</returns>
		public virtual GetBlockedUserResponse GetBlockedUsers(int? limit = null, string after = null, string before = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			if (limit.HasValue)
			{
				builder.Append($"?limit={limit}");
			}

			if (!string.IsNullOrWhiteSpace(after))
			{
				builder.Append($"?after={after}");
			}

			if (!string.IsNullOrWhiteSpace(before))
			{
				builder.Append($"?before={before}");
			}

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessGetAsync<GetBlockedUserResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Get a list of blocked users.
		/// </summary>
		/// <param name="limit"></param>
		/// <param name="after"></param>
		/// <param name="before"></param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>GetBlockedUserResponse</returns>
		public virtual async Task<GetBlockedUserResponse> GetBlockedUsersAsync(int? limit = null, string after = null, string before = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			if (limit.HasValue)
			{
				builder.Append($"?limit={limit}");
			}

			if (!string.IsNullOrWhiteSpace(after))
			{
				builder.Append($"?after={after}");
			}

			if (!string.IsNullOrWhiteSpace(before))
			{
				builder.Append($"?before={before}");
			}

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessGetAsync<GetBlockedUserResponse>(formattedWhatsAppEndpoint, cancellationToken);
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
		public virtual ConversationAnalyticsResponse GetConversationAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
            conversationAnalyticUrlBuilder.Append($".phone_numbers({JsonSerializer.Serialize(phoneNumbers)})");
            conversationAnalyticUrlBuilder.Append($".metric_types({JsonSerializer.Serialize(metricTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_types({JsonSerializer.Serialize(conversationTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_directions({JsonSerializer.Serialize(conversationDirections)})");
            conversationAnalyticUrlBuilder.Append($".dimensions({JsonSerializer.Serialize(dimensions)})");
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
        public virtual async Task<ConversationAnalyticsResponse> GetConversationAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
            conversationAnalyticUrlBuilder.Append($".phone_numbers({JsonSerializer.Serialize(phoneNumbers)})");
            conversationAnalyticUrlBuilder.Append($".metric_types({JsonSerializer.Serialize(metricTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_types({JsonSerializer.Serialize(conversationTypes)})");
            conversationAnalyticUrlBuilder.Append($".conversation_directions({JsonSerializer.Serialize(conversationDirections)})");
            conversationAnalyticUrlBuilder.Append($".dimensions({JsonSerializer.Serialize(dimensions)})");
			conversationAnalyticUrlBuilder.Append($"&access_token={_whatsAppConfig.AccessToken}");

			formattedWhatsAppEndpoint = conversationAnalyticUrlBuilder.ToString();

            return await WhatsAppBusinessGetAsync<ConversationAnalyticsResponse>(formattedWhatsAppEndpoint, cancellationToken, isHeaderAccessTokenProvided: false);
        }

        public virtual ConversationalComponentResponse GetConversationalMessage(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetConversationAutomation.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<ConversationalComponentResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public virtual async Task<ConversationalComponentResponse> GetConversationalMessageAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Get the call permission state for a specific phone number and consumer WhatsApp ID.
		/// </summary>
		/// <param name="phoneNumber">WhatsApp Phone Number Id</param>
		/// <param name="consumerWhatsAppID">Consumer WhatsApp Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Call permission state response</returns>
		public virtual async Task<CallPermissionStateResponse> GetCallPermissionStateAsync(string phoneNumber, string consumerWhatsAppID, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CallPermissionState
				.Replace("{{Phone-Number-ID}}", phoneNumber)
                .Replace("{{Consumer-WhatsApp-ID}}", consumerWhatsAppID);

            return await WhatsAppBusinessGetAsync<CallPermissionStateResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Get the call permission state for a specific phone number and consumer WhatsApp ID.
		/// </summary>
		/// <param name="phoneNumber">WhatsApp Phone Number Id</param>
		/// <param name="consumerWhatsAppID">Consumer WhatsApp Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Call permission state response</returns>
		public virtual CallPermissionStateResponse GetCallPermissionState(string phoneNumber, string consumerWhatsAppID, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.CallPermissionState
				.Replace("{{Phone-Number-ID}}", phoneNumber)
                .Replace("{{Consumer-WhatsApp-ID}}", consumerWhatsAppID);

            return WhatsAppBusinessGetAsync<CallPermissionStateResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

		public virtual async Task<WhatsAppGroupResponse> GetGroupJoinRequestsAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public virtual WhatsAppGroupResponse GetGroupJoinRequests(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// To retrieve your media’s URL, make a GET call to /{{Media-ID}}. Later, you can use this URL to download the media file.
		/// </summary>
		/// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
		/// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>MediaUrlResponse</returns>
		public virtual MediaUrlResponse GetMediaUrl(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
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
        public virtual async Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default)
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
		/// Synchronizing messaging history
		/// </summary>
		/// <param name="messageHistoryRequest">messageHistoryRequest object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns>MessageHistoryResponse</returns>
		public virtual MessageHistoryResponse GetMessageHistorySynchronization(MessageHistoryRequest messageHistoryRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.MessageHistorySync);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessPostAsync<MessageHistoryResponse>(messageHistoryRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Synchronizing messaging history
		/// </summary>
		/// <param name="messageHistoryRequest">messageHistoryRequest object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">cancellation token</param>
		/// <returns>MessageHistoryResponse</returns>
		public virtual async Task<MessageHistoryResponse> GetMessageHistorySynchronizationAsync(MessageHistoryRequest messageHistoryRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.MessageHistorySync);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
            return await WhatsAppBusinessPostAsync<MessageHistoryResponse>(messageHistoryRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// To get a list of all the QR codes messages for a business
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>QRCodeMessageFilterResponse</returns>
		public virtual QRCodeMessageFilterResponse GetQRCodeMessageList(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<QRCodeMessageFilterResponse> GetQRCodeMessageListAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual QRCodeMessageFilterResponse GetQRCodeMessageById(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<QRCodeMessageFilterResponse> GetQRCodeMessageByIdAsync(string qrCodeId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual SharedWABAIDResponse GetSharedWABAId(string inputToken, CancellationToken cancellationToken = default)
        {
            return WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID.Replace("{{Input-Token}}", inputToken), cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        public virtual async Task<SharedWABAIDResponse> GetSharedWABAIdAsync(string inputToken, CancellationToken cancellationToken = default)
        {
            return await WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID.Replace("{{Input-Token}}", inputToken), cancellationToken);
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        public virtual SharedWABAResponse GetSharedWABAList(string businessId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Business public virtual Key
        /// </summary>
        /// <remarks>You must have the whatsapp_business_messaging permission</remarks>
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsappBusinessEncryptionResponse</returns>
        public virtual async Task<WhatsappBusinessEncryptionResponse> GetWhatsappBusinessEncryptionAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
	        if (cloudApiConfig is not null)
	        {
		        _whatsAppConfig = cloudApiConfig;
	        }
	        
	        var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.WhatsAppBusinessEncryption.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
	        return await WhatsAppBusinessGetAsync<WhatsappBusinessEncryptionResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Get Business public virtual Key
        /// </summary>
        /// <remarks>You must have the whatsapp_business_messaging permission</remarks>
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsappBusinessEncryptionResponse</returns>
        public virtual WhatsappBusinessEncryptionResponse GetWhatsappBusinessEncryption(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
	        if (cloudApiConfig is not null)
	        {
		        _whatsAppConfig = cloudApiConfig;
	        }
	        
	        var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.WhatsAppBusinessEncryption.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
	        return WhatsAppBusinessGetAsync<WhatsappBusinessEncryptionResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Set Business public virtual Key
        /// </summary>
        /// <remarks>You must have the whatsapp_business_messaging permission</remarks>
        /// <param name="publicKey">The 2048-bit RSA business public virtual key generated</param>
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsappBusinessEncryptionResponse</returns>
        public virtual async Task<BaseSuccessResponse> SetWhatsappBusinessEncryptionAsync(string publicKey, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
	        if (cloudApiConfig is not null)
	        {
		        _whatsAppConfig = cloudApiConfig;
	        }
	        
	        var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.WhatsAppBusinessEncryption.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
	        
	        var formBody = new List<KeyValuePair<string, string>>();
	        formBody.Add(new KeyValuePair<string, string>("business_public_key", publicKey));
	        
	        return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formBody, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Set Business public virtual Key
        /// </summary>
        /// <remarks>You must have the whatsapp_business_messaging permission</remarks>
        /// <param name="publicKey">The 2048-bit RSA business public virtual key generated</param>
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsappBusinessEncryptionResponse</returns>
        public virtual BaseSuccessResponse SetWhatsappBusinessEncryption(string publicKey, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
	        if (cloudApiConfig is not null)
	        {
		        _whatsAppConfig = cloudApiConfig;
	        }
	        
	        var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.WhatsAppBusinessEncryption.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
	        
	        var formBody = new List<KeyValuePair<string, string>>();
	        formBody.Add(new KeyValuePair<string, string>("business_public_key", publicKey));
	        
	        return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formBody, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        public virtual async Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return await WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// Get detailed WhatsApp Business Account information by WABA ID
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">WhatsApp Business Account ID</param>
        /// <param name="cloudApiConfig">Cloud API configuration</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WABADetailsResponse</returns>
        public virtual WABADetailsResponse GetWABADetails(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetWABADetails.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<WABADetailsResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get detailed WhatsApp Business Account information by WABA ID
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">WhatsApp Business Account ID</param>
        /// <param name="cloudApiConfig">Cloud API configuration</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WABADetailsResponse</returns>
        public virtual async Task<WABADetailsResponse> GetWABADetailsAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetWABADetails.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<WABADetailsResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupInviteLinkResponse> GetWhatsAppGroupInviteLinkAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupInviteLink.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessGetAsync<WhatsAppGroupInviteLinkResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupInviteLinkResponse GetWhatsAppGroupInviteLink(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupInviteLink.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessGetAsync<WhatsAppGroupInviteLinkResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Get Whatsapp template message by namespace
		/// </summary>
		/// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>TemplateNamespaceResponse</returns>
		public virtual async Task<TemplateNamespaceResponse> GetTemplateNamespaceAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual TemplateNamespaceResponse GetTemplateNamespace(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<TemplateByIdResponse> GetTemplateByIdAsync(string templateId, CancellationToken cancellationToken = default)
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
		public virtual TemplateByIdResponse GetTemplateById(string templateId, CancellationToken cancellationToken = default)
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
		public virtual async Task<TemplateByNameResponse> GetTemplateByNameAsync(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual TemplateByNameResponse GetTemplateByName(string whatsAppBusinessAccountId, string templateName, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="pagingUrl">Cursor paging url</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>TemplateResponse</returns>
        public virtual async Task<TemplateResponse> GetAllTemplatesAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, string pagingUrl = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetAllTemplateMessage);
            builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

            var formattedWhatsAppEndpoint = builder.ToString();

            if (string.IsNullOrWhiteSpace(pagingUrl))
            {
                return await WhatsAppBusinessGetAsync<TemplateResponse>(formattedWhatsAppEndpoint, cancellationToken);
            }
            else
            {
				return await WhatsAppBusinessGetAsync<TemplateResponse>(pagingUrl, cancellationToken);
			}
        }

        /// <summary>
        /// Get All templates for the whatsapp business account
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Whatsapp Business Account Id</param>
        /// <param name="cloudApiConfig">Custom cloudapi config</param>
        /// <param name="pagingUrl">Cursor paging url</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>TemplateResponse</returns>
        public virtual TemplateResponse GetAllTemplates(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, string pagingUrl = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.GetAllTemplateMessage);
            builder.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);

            var formattedWhatsAppEndpoint = builder.ToString();

            if (string.IsNullOrWhiteSpace(pagingUrl))
            {
                return WhatsAppBusinessGetAsync<TemplateResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
            }
            else
            {
				return WhatsAppBusinessGetAsync<TemplateResponse>(pagingUrl, cancellationToken).GetAwaiter().GetResult();
			}
        }

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        public virtual SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupResponse> GetWhatsAppGroupInfoAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse GetWhatsAppGroupInfo(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		public virtual async Task<WhatsAppGroupResponse> GetActiveWhatsAppGroupsAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Groups.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse GetActiveWhatsAppGroups(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Groups.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<WhatsAppGroupResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Initiate a Business Initiated Call to WhatsApp user.
		/// </summary>
		/// <param name="callRequest">Call request object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppCallResponse</returns>
		public virtual async Task<WhatsAppCallResponse> InitiateWhatsAppCallAsync(CallRequest callRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Calls.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppCallResponse>(callRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Initiate a Business Initiated Call to WhatsApp user.
		/// </summary>
		/// <param name="callRequest">Call request object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppCallResponse</returns>
		public virtual WhatsAppCallResponse InitiateWhatsAppCall(CallRequest callRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Calls.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppCallResponse>(callRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Pre accept, accept, reject or Terminate a Business Initiated Call to WhatsApp user.
		/// </summary>
		/// <param name="callRequest">Call request object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual async Task<BaseSuccessResponse> ManageWhatsAppCallActionAsync(CallRequest callRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Calls.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(callRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Pre accept, accept, reject or Terminate a Business Initiated Call to WhatsApp user.
		/// </summary>
		/// <param name="callRequest">Call request object</param>
		/// <param name="cloudApiConfig">custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual BaseSuccessResponse ManageWhatsAppCallAction(CallRequest callRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.Calls.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(callRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		#region OAuth functions
		/// <summary>
		/// Exchanges an authorization code for an access token asynchronously.
		/// This is used as part of the OAuth 2.0 authorization code flow for Meta Embedded Signup.
		/// </summary>
		/// <param name="exchangeTokenRequest">The exchange token request containing the authorization code and client credentials</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>ExchangeTokenResponse containing the access token or error information</returns>
		public virtual async Task<ExchangeTokenResponse> ExchangeTokenAsync(ExchangeTokenRequest exchangeTokenRequest, CancellationToken cancellationToken = default)
		{
			var formData = new Dictionary<string, string>
			{
				{ "grant_type", exchangeTokenRequest.GrantType },
				{ "client_id", exchangeTokenRequest.ClientId },
				{ "client_secret", exchangeTokenRequest.ClientSecret },
				{ "code", exchangeTokenRequest.Code }
			};

			if (!string.IsNullOrEmpty(exchangeTokenRequest.RedirectUri))
			{
				formData.Add("redirect_uri", exchangeTokenRequest.RedirectUri);
			}

			var formContent = new FormUrlEncodedContent(formData);
			var request = new HttpRequestMessage(HttpMethod.Post, WhatsAppBusinessRequestEndpoint.OAuthAccessToken)
			{
				Content = formContent
			};

			var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
			var responseContent = await response.Content.ReadAsStringAsync();

			var result = JsonSerializer.Deserialize<ExchangeTokenResponse>(responseContent);
			
			if (!response.IsSuccessStatusCode && result?.Error != null)
			{
				throw new WhatsappBusinessCloudAPIException(
					response.StatusCode,
					$"OAuth token exchange failed: {result.Error} - {result.ErrorDescription}"
				);
			}

			return result ?? new ExchangeTokenResponse();
		}

		/// <summary>
		/// Exchanges an authorization code for an access token synchronously.
		/// This is used as part of the OAuth 2.0 authorization code flow for Meta Embedded Signup.
		/// </summary>
		/// <param name="exchangeTokenRequest">The exchange token request containing the authorization code and client credentials</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>ExchangeTokenResponse containing the access token or error information</returns>
		public virtual ExchangeTokenResponse ExchangeToken(ExchangeTokenRequest exchangeTokenRequest, CancellationToken cancellationToken = default)
		{
			return ExchangeTokenAsync(exchangeTokenRequest, cancellationToken).GetAwaiter().GetResult();
		}
		#endregion

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
		public virtual MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MigrateAccount.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(migrateAccountRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public virtual async Task<WhatsAppResponse> PinGroupMessageAsync(PinGroupMessageRequest pinGroupMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(pinGroupMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		public virtual WhatsAppResponse PinGroupMessage(PinGroupMessageRequest pinGroupMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(pinGroupMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
		/// When uploading data, you must include the access token as an HTTP header.
		/// </summary>
		/// <param name="uploadId">Upload session</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>ResumableUploadResponse</returns>
		public virtual async Task<ResumableUploadResponse> QueryFileUploadStatusAsync(string uploadId, CancellationToken cancellationToken = default)
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
        public virtual ResumableUploadResponse QueryFileUploadStatus(string uploadId, CancellationToken cancellationToken = default)
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
        public virtual BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupJoinRequestResponse> RejectJoinRequestsAsync(GroupJoinRequest groupJoinRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessDeleteAsync<WhatsAppGroupJoinRequestResponse>(groupJoinRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupJoinRequestResponse RejectJoinRequests(GroupJoinRequest groupJoinRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupJoinRequests.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessDeleteAsync<WhatsAppGroupJoinRequestResponse>(groupJoinRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		public virtual async Task<WhatsAppGroupResponse> RemoveWhatsAppGroupParticipantsAsync(GroupRequest groupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RemoveGroupParticipant.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessDeleteAsync<WhatsAppGroupResponse>(groupRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse RemoveWhatsAppGroupParticipants(GroupRequest groupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RemoveGroupParticipant.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessDeleteAsync<WhatsAppGroupResponse>(groupRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified below.
		/// To verify a phone number using Graph API, make a POST request to {{PHONE_NUMBER_ID
		/// }}/ request_code.In your call, include your chosen verification method and locale. You need to authenticate yourself using { { User - Access - Token} } (This is automatically done for you in the Request Verification Code request).
		/// </summary>
		/// <param name="requestVerification">RequestVerificationCode object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>VerificationResponse</returns>
		public virtual VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken);
        }

		public virtual async Task<WhatsAppGroupInviteLinkResponse> ResetWhatsAppGroupInviteLinkAsync(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupInviteLink.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return await WhatsAppBusinessPostAsync<WhatsAppGroupInviteLinkResponse>(new GroupRequest(), formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupInviteLinkResponse ResetWhatsAppGroupInviteLink(WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupInviteLink.Replace("{{Group-ID}}", _whatsAppConfig.GroupId);
            return WhatsAppBusinessPostAsync<WhatsAppGroupInviteLinkResponse>(new GroupRequest(), formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Audio Message using Media Id
		/// </summary>
		/// <param name="audioMessage">Audio Message Object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendDocumentAttachmentTemplateMessageAsync(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendDocumentAttachmentTemplateMessage(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendInteractiveCTAButtonMessage(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendInteractiveCTAButtonMessageAsync(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendLocationTemplateMessage(LocationTemplateMessageRequest locationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendLocationTemplateMessageAsync(LocationTemplateMessageRequest locationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendReactionMessageReply(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendReactionMessageReplyAsync(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendVideoAttachmentTemplateMessage(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendVideoAttachmentTemplateMessageAsync(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendSingleProductMessageAsync(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendSingleProductMessage(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendMultipleProductMessageAsync(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendMultipleProductMessage(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendAuthenticationMessageTemplateAsync(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendAuthenticationMessageTemplate(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendMPMTemplateAsync(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendMPMTemplate(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(multiProductTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send SPM Template Message
		/// </summary>
		/// <param name="singleProductTemplateMessageRequest">SingleProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendSPMTemplateAsync(SingleProductTemplateMessageRequest singleProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send SPM Template Message
		/// </summary>
		/// <param name="singleProductTemplateMessageRequest">SingleProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendSPMTemplate(SingleProductTemplateMessageRequest singleProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(singleProductTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Catalog Template Message
		/// </summary>
		/// <param name="catalogTemplateMessageRequest">CatalogTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendCatalogMessageTemplateAsync(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendCatalogMessageTemplate(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendCatalogMessageAsync(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendCatalogMessage(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendCarouselMessageTemplateAsync(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendCarouselMessageTemplate(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendCouponCodeMessageTemplateAsync(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendCouponCodeMessageTemplate(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendLimitedTimeOfferMessageTemplateAsync(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendLimitedTimeOfferMessageTemplate(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<FlowMessageResponse> SendFlowMessageAsync(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual FlowMessageResponse SendFlowMessage(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendFlowMessageTemplateAsync(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendFlowMessageTemplate(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<WhatsAppResponse> SendGenericMessageAsync(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual WhatsAppResponse SendGenericMessage(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual async Task<WhatsAppResponse> SendLocationRequestMessageAsync(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
		public virtual WhatsAppResponse SendLocationRequestMessage(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveLocationMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Template Message based on parameters
		/// </summary>
		/// <param name="recipientPhoneNumber">Recipient Phone Number</param>
		/// <param name="templateName">Name of the template</param>
		/// <param name="languageCode">Language Code of the template</param>
		/// <param name="components">Components for the template</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendTemplateMessageAsync(string recipientPhoneNumber, string templateName, string languageCode, TemplateComponent[] components = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			var processedComponents = new List<object>();

			if (components != null)
			{
				foreach (var component in components)
				{
					var componentObject = new Dictionary<string, object>
			        {
				        { "type", component.Type }
			        };

					// Add "text" if present (for text-based headers and footers)
					if (!string.IsNullOrEmpty(component.Text))
					{
						componentObject["text"] = component.Text;
					}

					// Add "parameters" if present
					if (component.Parameters != null && component.Parameters.Length > 0)
					{
						componentObject["parameters"] = component.Parameters;
					}

					processedComponents.Add(componentObject);
				}
			}

			var payload = new
			{
				messaging_product = "whatsapp",
				recipient_type = "individual",
				to = recipientPhoneNumber,
				type = "template",
				template = new
				{
					name = templateName,
					language = new { code = languageCode },
					components = processedComponents.Count > 0 ? processedComponents.ToArray() : null
				}
			};

			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(payload, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Template Message based on parameters
		/// </summary>
		/// <param name="recipientPhoneNumber">Recipient Phone Number</param>
		/// <param name="templateName">Name of the template</param>
		/// <param name="languageCode">Language Code of the template</param>
		/// <param name="components">Components for the template</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendTemplateMessage(string recipientPhoneNumber, string templateName, string languageCode, TemplateComponent[] components = null, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
			var processedComponents = new List<object>();

			if (components != null)
			{
				foreach (var component in components)
				{
					var componentObject = new Dictionary<string, object>
					{
						{ "type", component.Type }
					};

					// Add "text" if present (for text-based headers and footers)
					if (!string.IsNullOrEmpty(component.Text))
					{
						componentObject["text"] = component.Text;
					}

					// Add "parameters" if present
					if (component.Parameters != null && component.Parameters.Length > 0)
					{
						componentObject["parameters"] = component.Parameters;
					}

					processedComponents.Add(componentObject);
				}
			}

			var payload = new
			{
				messaging_product = "whatsapp",
				recipient_type = "individual",
				to = recipientPhoneNumber,
				type = "template",
				template = new
				{
					name = templateName,
					language = new { code = languageCode },
					components = processedComponents.Count > 0 ? processedComponents.ToArray() : null
				}
			};

			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(payload, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Free Form Call Permission Message
		/// </summary>
		/// <param name="freeFormCallPermissionMessageRequest">FreeFormCallPermissionMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendFreeFormCallPermissionMessageAsync(FreeFormCallPermissionMessageRequest freeFormCallPermissionMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return await WhatsAppBusinessPostAsync<WhatsAppResponse>(freeFormCallPermissionMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Send Free Form Call Permission Message
		/// </summary>
		/// <param name="freeFormCallPermissionMessageRequest">FreeFormCallPermissionMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendFreeFormCallPermissionMessage(FreeFormCallPermissionMessageRequest freeFormCallPermissionMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
			return WhatsAppBusinessPostAsync<WhatsAppResponse>(freeFormCallPermissionMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

		/// <summary>
		/// Send Call Permission Template Message
		/// </summary>
		/// <param name="callPermissionTemplateMessageRequest">CallPermissionTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendCallPermissionTemplateMessageAsync(CallPermissionTemplateMessageRequest callPermissionTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(callPermissionTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Send Call Permission Template Message
		/// </summary>
		/// <param name="callPermissionTemplateMessageRequest">CallPermissionTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendCallPermissionTemplateMessage(CallPermissionTemplateMessageRequest callPermissionTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(callPermissionTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Send Voice Call Message
		/// </summary>
		/// <param name="voiceCallMessageRequest">voice call message request object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual async Task<WhatsAppResponse> SendVoiceCallMessageAsync(VoiceCallMessageRequest voiceCallMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(voiceCallMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Send Voice Call Message
		/// </summary>
		/// <param name="voiceCallMessageRequest">voice call message request object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		public virtual WhatsAppResponse SendVoiceCallMessage(VoiceCallMessageRequest voiceCallMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(voiceCallMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

		/// <summary>
		/// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
		/// You set up two-factor verification and register a phone number in the same API call.
		/// </summary>
		/// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetTwoFactor.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(twoStepVerificationRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

		/// <summary>
		/// Use this endpoint to unblock a list of WhatsApp user numbers.
		/// </summary>
		/// <param name="blockUserRequest">Block User Request</param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BlockUserResponse</returns>
		public virtual BlockUserResponse UnblockUser(BlockUserRequest blockUserRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
			return WhatsAppBusinessDeleteAsync<BlockUserResponse>(blockUserRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Use this endpoint to unblock a list of WhatsApp user numbers.
		/// </summary>
		/// <param name="blockUserRequest">Block User Request</param>
		/// <param name="cloudApiConfig">Custom cloud api config</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BlockUserResponse</returns>
		public virtual async Task<BlockUserResponse> UnblockUserAsync(BlockUserRequest blockUserRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
		{
			if (cloudApiConfig is not null)
			{
				_whatsAppConfig = cloudApiConfig;
			}

			var builder = new StringBuilder();

			builder.Append(WhatsAppBusinessRequestEndpoint.BlockUser);
			builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

			var formattedWhatsAppEndpoint = builder.ToString();
			return await WhatsAppBusinessDeleteAsync<BlockUserResponse>(blockUserRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

		/// <summary>
		/// Update the business profile information such as the business description, email or address. To update your profile, make a POST call to /{{Phone-Number-ID}}/whatsapp_business_profile. In your request, you can include the parameters listed below.
		/// It is recommended that you use Resumable Upload - Create an Upload Session to obtain an upload ID.Then use this upload ID in a call to Resumable Upload - Upload File Data to obtain the picture handle.This handle can be used for the profile_picture_handle
		/// </summary>
		/// <param name="updateBusinessProfileRequest">UpdateBusinessProfileRequest object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		public virtual BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual QRCodeMessageResponse UpdateQRCodeMessage(string qrCodeId, string messageText, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.UpdateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();

			QRCodeMessageRequest qRCodeMessageRequest = new QRCodeMessageRequest
			{
				PrefilledMessage = messageText,
				Code = qrCodeId
			};

			return WhatsAppBusinessPostAsync<QRCodeMessageResponse>(qRCodeMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To update a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls/{qr-code-id} endpoint and include the parameter you wish to update.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="messageText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        public virtual async Task<QRCodeMessageResponse> UpdateQRCodeMessageAsync(string qrCodeId, string messageText, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var builder = new StringBuilder();

            builder.Append(WhatsAppBusinessRequestEndpoint.UpdateQRCodeMessage);
            builder.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);

            var formattedWhatsAppEndpoint = builder.ToString();

			QRCodeMessageRequest qRCodeMessageRequest = new QRCodeMessageRequest
			{
				PrefilledMessage = messageText,
				Code = qrCodeId
			};

			return await WhatsAppBusinessPostAsync<QRCodeMessageResponse>(qRCodeMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public virtual async Task<ResumableUploadResponse> UploadFileDataAsync(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default)
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
        public virtual ResumableUploadResponse UploadFileData(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadFileData.Replace("{{Upload-ID}}", uploadId);
            return WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, filePath, fileContentType, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="fileData">Full file content data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public virtual ResumableUploadResponse UploadFileData(string uploadId, string fileName, string fileContentType, byte[] fileData, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadFileData.Replace("{{Upload-ID}}", uploadId);
            return WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, fileName, fileContentType, fileData, cancellationToken).GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="fileData">Full file content data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        public virtual async Task<ResumableUploadResponse> UploadFileDataAsync(string uploadId, string fileName, string fileContentType, byte[] fileData, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.ResumableUploadFileData.Replace("{{Upload-ID}}", uploadId);
            return await WhatsAppBusinessPostAsync<ResumableUploadResponse>(formattedWhatsAppEndpoint, fileName, fileContentType, fileData, cancellationToken);
        }

        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public virtual MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaRequest.File, uploadMediaRequest.Type, cancellationToken, true);
        }
        
        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaDataRequest">UploadMediaDataRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public virtual MediaUploadResponse UploadMedia(UploadMediaDataRequest uploadMediaDataRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaDataRequest.FileName, uploadMediaDataRequest.Type, uploadMediaDataRequest.Data, cancellationToken, true).GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaDataRequest">UploadMediaDataRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        public virtual async Task<MediaUploadResponse> UploadMediaAsync(UploadMediaDataRequest uploadMediaDataRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MediaUploadResponse>(formattedWhatsAppEndpoint, uploadMediaDataRequest.FileName, uploadMediaDataRequest.Type, uploadMediaDataRequest.Data, cancellationToken, true);
        }

		public virtual async Task<WhatsAppGroupResponse> UpdateWhatsAppGroupSettingsAsync(GroupRequest groupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }

            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppGroupResponse>(groupRequest, formattedWhatsAppEndpoint, cancellationToken);
		}

        public virtual WhatsAppGroupResponse UpdateWhatsAppGroupSettings(GroupRequest groupRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
        {
            if (cloudApiConfig is not null)
            {
                _whatsAppConfig = cloudApiConfig;
            }
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GroupDetails.Replace("{{Group-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppGroupResponse>(groupRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
		}

		/// <summary>
		/// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
		/// </summary>
		/// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>VerificationResponse</returns>
		public virtual VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
        public virtual async Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default)
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
            string json = JsonSerializer.Serialize(whatsAppDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
			}
			return result;
        }
        
        /// <summary>
        /// To perform WhatsApp Business Cloud API functions
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="formValues">Key/Value pairs of items to form URL encode in the request body</param>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business CLoud API endpoint</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response Object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPostAsync<T>(IEnumerable<KeyValuePair<string,string>> formValues, string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
            T result = new();
            
            var content = new FormUrlEncodedContent(formValues);
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
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
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
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
            var file = new FileInfo(filePath);
#if NET5_0_OR_GREATER
            var fileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
#else
            var fileData = File.ReadAllBytes(filePath);
#endif
            return await WhatsAppBusinessPostAsync<T>(whatsAppBusinessEndpoint, file.FullName, fileContentType, fileData, cancellationToken, isMediaUpload);
        }

        /// <summary>
        /// To upload a profile picture to your business profile and media upload.
        /// </summary>
        /// <typeparam name="T">Response Class</typeparam>
        /// <param name="whatsAppBusinessEndpoint">WhatsApp Business Media Upload endpoint</param>
        /// <param name="fileName">File name</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="fileData">Full file content data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response object</returns>
        /// <exception cref="WhatsappBusinessCloudAPIException"></exception>
        private async Task<T> WhatsAppBusinessPostAsync<T>(string whatsAppBusinessEndpoint, string fileName, string fileContentType, byte[] fileData, CancellationToken cancellationToken = default, bool isMediaUpload = false) where T : new()
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

            var boundary = $"----------{Guid.NewGuid():N}";
            var content = new MultipartFormDataContent(boundary);

			HttpResponseMessage? response;

			if (isMediaUpload)
            {
                var mediaFileContent = new ByteArrayContent(fileData);
                mediaFileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileName,
                };
                mediaFileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileContentType);

                var fileValues = new
                {
                    messaging_product = "whatsapp"
                };

                content.Add(mediaFileContent);
                content.Add(new StringContent(fileValues.messaging_product), "messaging_product");

				response = await _httpClient.PostAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);
			}
            else // Resumable upload
            {
				var mediaFileContent = new ByteArrayContent(fileData);

                var requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = mediaFileContent;
                requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

				requestMessage.RequestUri = new Uri($"{_httpClient.BaseAddress}{whatsAppBusinessEndpoint}");

				response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
			}

			if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
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
            if (!_httpClient.DefaultRequestHeaders.UserAgent.Contains(productValue))
            {
                _httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);
            }

#if NET5_0_OR_GREATER
            var bytesDownloaded = await _httpClient.GetByteArrayAsync(whatsAppBusinessEndpoint, cancellationToken).ConfigureAwait(false);
#elif NETSTANDARD2_0_OR_GREATER || NET472
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
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
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
            string json = JsonSerializer.Serialize(whatsAppDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _httpClient.PutAsync(whatsAppBusinessEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
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
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
            {
                WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
			}
			return result;
        }

		private async Task<T> WhatsAppBusinessDeleteAsync<T>(object whatsAppDto, string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default, bool isHeaderAccessTokenProvided = true) where T : new()
		{
			if (isHeaderAccessTokenProvided)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
			}
			
            T result = new();
			
            string json = JsonSerializer.Serialize(whatsAppDto);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			
            cancellationToken.ThrowIfCancellationRequested();

			var request = new HttpRequestMessage(HttpMethod.Delete, whatsAppBusinessEndpoint);

            request.Content = content;

			var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

			if (response.IsSuccessStatusCode)
			{
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
				}
#endif
			}
			else
			{
				WhatsAppErrorResponse whatsAppErrorResponse = new WhatsAppErrorResponse();
#if NET5_0_OR_GREATER
				using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
				throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
#if NETSTANDARD2_0_OR_GREATER || NET472
                using (var stream = await response.Content.ReadAsStreamAsync())
				{
					whatsAppErrorResponse = await JsonSerializer.DeserializeAsync<WhatsAppErrorResponse>(stream, cancellationToken: cancellationToken);
				}
                throw new WhatsappBusinessCloudAPIException(new HttpRequestException(whatsAppErrorResponse.Error.Message), response.StatusCode, whatsAppErrorResponse);
#endif
			}
			return result;
		}
	}
}
