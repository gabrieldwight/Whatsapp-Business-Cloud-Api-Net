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
    public class WhatsAppBusinessClient : IWhatsAppBusinessClient
    {
        private readonly HttpClient _httpClient;
        readonly Random jitterer = new Random();
        private readonly JsonSerializer _serializer = new JsonSerializer();
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;

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

        public WhatsAppBusinessClient(HttpClient httpClient, WhatsAppBusinessCloudApiConfig whatsAppConfig)
        {
            _httpClient = httpClient;
            _whatsAppConfig = whatsAppConfig;
        }

        public BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SubscribeAppToWABA.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse DeleteMedia(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteMedia.Replace("{{Media-ID}}", mediaId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeleteSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessDeleteAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.DeregisterPhone.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetBusinessProfileId.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<BusinessProfileResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public MediaUrlResponse GetMediaUrl(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            return WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetMediaUrl.Replace("{{Media-ID}}", mediaId);
            return await WhatsAppBusinessGetAsync<MediaUrlResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public SharedWABAIDResponse GetSharedWABAId(CancellationToken cancellationToken = default)
        {
            return WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<SharedWABAIDResponse> GetSharedWABAIdAsync(CancellationToken cancellationToken = default)
        {
            return await WhatsAppBusinessGetAsync<SharedWABAIDResponse>(WhatsAppBusinessRequestEndpoint.GetSharedWABAID, cancellationToken);
        }

        public SharedWABAResponse GetSharedWABAList(string businessId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetListSharedWABA.Replace("{{Business-ID}}", businessId);
            return await WhatsAppBusinessGetAsync<SharedWABAResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetSubscribedApps.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<SubscribedAppsResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumberById.Replace("{{Phone-Number-ID}}", whatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessGetAsync<PhoneNumberByIdResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public async Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.GetPhoneNumbers.Replace("{{WABA-ID}}", whatsAppBusinessAccountId);
            return await WhatsAppBusinessGetAsync<PhoneNumberResponse>(formattedWhatsAppEndpoint, cancellationToken);
        }

        public MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MarkMessageAsRead.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MarkMessageResponse>(markMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MarkMessageAsRead.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MarkMessageResponse>(markMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MigrateAccount.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(migrateAccountRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.MigrateAccount.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(migrateAccountRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RegisterPhone.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(registerPhoneRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.RequestVerificationCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(requestVerification, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(audioMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(contactMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(documentMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(imageTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveListMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveReplyButtonMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(interactiveTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(locationMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(stickerMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(textTemplateMessageRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SendMessage.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<WhatsAppResponse>(videoMessage, formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetTwoFactor.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(twoStepVerificationRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.SetTwoFactor.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(twoStepVerificationRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UpdateBusinessProfileId.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<BaseSuccessResponse>(updateBusinessProfileRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UpdateBusinessProfileId.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<BaseSuccessResponse>(updateBusinessProfile, formattedWhatsAppEndpoint, cancellationToken);
        }

        public MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<MediaUploadResponse>(uploadMediaRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.UploadMedia.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<MediaUploadResponse>(uploadMediaRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

        public VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.VerifyCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return WhatsAppBusinessPostAsync<VerificationResponse>(verifyCodeRequest, formattedWhatsAppEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        public async Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default)
        {
            var formattedWhatsAppEndpoint = WhatsAppBusinessRequestEndpoint.VerifyCode.Replace("{{Phone-Number-ID}}", _whatsAppConfig.WhatsAppBusinessPhoneNumberId);
            return await WhatsAppBusinessPostAsync<VerificationResponse>(verifyCodeRequest, formattedWhatsAppEndpoint, cancellationToken);
        }

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

        private async Task<T> WhatsAppBusinessGetAsync<T>(string whatsAppBusinessEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _whatsAppConfig.AccessToken);
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
