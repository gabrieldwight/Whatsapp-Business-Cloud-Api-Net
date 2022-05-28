using WhatsappBusiness.CloudApi.AccountMigration.Requests;
using WhatsappBusiness.CloudApi.BusinessProfile.Requests;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.PhoneNumbers.Requests;
using WhatsappBusiness.CloudApi.Registration.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.TwoStepVerification.Requests;

namespace WhatsappBusiness.CloudApi.Interfaces
{
    public interface IWhatsAppBusinessClient
    {
        #region Account Migration function
        Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Business Profile functions
        Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);
        
        BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Media functions
        Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, string accessToken, CancellationToken cancellationToken = default);
        
        MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, string accessToken, CancellationToken cancellationToken = default);

        MediaUrlResponse GetMediaUrl(string mediaId, string accessToken, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeleteMedia(string mediaId, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Phone Numbers functions
        Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);

        PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);

        Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, string accessToken, CancellationToken cancellationToken = default);

        VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, string accessToken, CancellationToken cancellationToken = default);

        Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, string accessToken, CancellationToken cancellationToken = default);

        VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Registration functions
        Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Send Messages functions
        Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, string accessToken, CancellationToken cancellationToken = default);
        
        WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, string accessToken, CancellationToken cancellationToken = default);
        
        Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, string accessToken, CancellationToken cancellationToken = default);

        MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, string accessToken, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, string accessToken, CancellationToken cancellationToken = default);

        WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region Two step verification code function
        Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region WhatsApp Business Account functions
        Task<SharedWABAIDResponse> GetSharedWABAIdAsync(string accessToken, CancellationToken cancellationToken = default);

        SharedWABAIDResponse GetSharedWABAId(string accessToken, CancellationToken cancellationToken = default);

        Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, string accessToken, CancellationToken cancellationToken = default);

        SharedWABAResponse GetSharedWABAList(string businessId, string accessToken, CancellationToken cancellationToken = default);
        #endregion

        #region WABA subscription functions
        Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);
        
        BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, string accessToken, CancellationToken cancellationToken = default);
        #endregion
    }
}
