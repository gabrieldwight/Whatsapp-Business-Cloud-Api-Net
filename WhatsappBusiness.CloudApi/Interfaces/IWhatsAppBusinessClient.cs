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
        Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default);

        BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Business Profile functions
        Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);
        
        BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, CancellationToken cancellationToken = default);

        BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Media functions
        Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default);
        
        MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default);

        Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, CancellationToken cancellationToken = default);

        MediaUrlResponse GetMediaUrl(string mediaId, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeleteMedia(string mediaId, CancellationToken cancellationToken = default);
        #endregion

        #region Phone Numbers functions
        Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default);

        VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default);

        Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default);

        VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Registration functions
        Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default);

        BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);
        #endregion

        #region Send Messages functions
        Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default);
        
        WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, CancellationToken cancellationToken = default);
        
        Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default);

        WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default);

        WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default);

        WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, CancellationToken cancellationToken = default);

        Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, CancellationToken cancellationToken = default);

        MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default);

        WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, CancellationToken cancellationToken = default);

        Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default);

        WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, CancellationToken cancellationToken = default);
        #endregion

        #region Two step verification code function
        Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default);

        BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default);
        #endregion

        #region WhatsApp Business Account functions
        Task<SharedWABAIDResponse> GetSharedWABAIdAsync(CancellationToken cancellationToken = default);

        SharedWABAIDResponse GetSharedWABAId(CancellationToken cancellationToken = default);

        Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, CancellationToken cancellationToken = default);

        SharedWABAResponse GetSharedWABAList(string businessId, CancellationToken cancellationToken = default);
        #endregion

        #region WABA subscription functions
        Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);
        
        BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);
        #endregion
    }
}
