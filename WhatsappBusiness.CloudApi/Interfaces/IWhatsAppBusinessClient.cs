using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.AccountMigration.Requests;
using WhatsappBusiness.CloudApi.BusinessProfile.Requests;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;
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
        /// <summary>
        /// To migrate your account, make a POST call to the /{{Phone-Number-ID}}/register endpoint and include the parameters listed below.
        /// Your request may take as long as 15 seconds to finish.During this period, your on-premise deployment is automatically disconnected from WhatsApp server and shutdown; the business account will start up in the cloud-hosted service at the same time. After the request finishes successfully, you can send messages immediately.
        /// </summary>
        /// <param name="migrateAccountRequest">MigrateAccountRequest Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> MigrateAccountAsync(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// To migrate your account, make a POST call to the /{{Phone-Number-ID}}/register endpoint and include the parameters listed below.
        /// Your request may take as long as 15 seconds to finish.During this period, your on-premise deployment is automatically disconnected from WhatsApp server and shutdown; the business account will start up in the cloud-hosted service at the same time. After the request finishes successfully, you can send messages immediately.
        /// </summary>
        /// <param name="migrateAccountRequest">MigrateAccountRequest Object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse MigrateAccount(MigrateAccountRequest migrateAccountRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Business Profile functions
        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        Task<BusinessProfileResponse> GetBusinessProfileIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// To complete the following API calls, you need to get a business profile ID. To do that, make a GET call to the /{{Phone-Number-ID}} endpoint and add whatsapp_business_profile as a URL field. Within the whatsapp_business_profile request, you can specify what you want to know from your business.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BusinessProfileResponse</returns>
        BusinessProfileResponse GetBusinessProfileId(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        /// <param name="fileLength">File length</param>
        /// <param name="fileType">File Content type</param>
        /// <param name="fileName">Full Path of the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        Task<ResumableUploadResponse> CreateResumableUploadSessionAsync(long fileLength, string fileContentType, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        /// <param name="fileLength">File length</param>
        /// <param name="fileType">File Content type</param>
        /// <param name="fileName">Full Path of the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        ResumableUploadResponse CreateResumableUploadSession(long fileLength, string fileContentType, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        Task<ResumableUploadResponse> UploadFileDataAsync(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        /// <param name="uploadId">Upload id Session</param>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContentType">File content type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        ResumableUploadResponse UploadFileData(string uploadId, string filePath, string fileContentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
        /// When uploading data, you must include the access token as an HTTP header.
        /// </summary>
        /// <param name="uploadId">Upload session</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        Task<ResumableUploadResponse> QueryFileUploadStatusAsync(string uploadId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
        /// When uploading data, you must include the access token as an HTTP header.
        /// </summary>
        /// <param name="uploadId">Upload session</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>ResumableUploadResponse</returns>
        ResumableUploadResponse QueryFileUploadStatus(string uploadId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the business profile information such as the business description, email or address. To update your profile, make a POST call to /{{Phone-Number-ID}}/whatsapp_business_profile. In your request, you can include the parameters listed below.
        /// It is recommended that you use Resumable Upload - Create an Upload Session to obtain an upload ID.Then use this upload ID in a call to Resumable Upload - Upload File Data to obtain the picture handle.This handle can be used for the profile_picture_handle
        /// </summary>
        /// <param name="updateBusinessProfileRequest">UpdateBusinessProfileRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> UpdateBusinessProfileAsync(UpdateBusinessProfileRequest updateBusinessProfile, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the business profile information such as the business description, email or address. To update your profile, make a POST call to /{{Phone-Number-ID}}/whatsapp_business_profile. In your request, you can include the parameters listed below.
        /// It is recommended that you use Resumable Upload - Create an Upload Session to obtain an upload ID.Then use this upload ID in a call to Resumable Upload - Upload File Data to obtain the picture handle.This handle can be used for the profile_picture_handle
        /// </summary>
        /// <param name="updateBusinessProfileRequest">UpdateBusinessProfileRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse UpdateBusinessProfile(UpdateBusinessProfileRequest updateBusinessProfileRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Media functions
        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        Task<MediaUploadResponse> UploadMediaAsync(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload Media: Image, Document, Audio, Video, Sticker
        /// </summary>
        /// <param name="uploadMediaRequest">UploadMediaRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUploadResponse</returns>
        MediaUploadResponse UploadMedia(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}?phone_number_id=<PHONE_NUMBER_ID>. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        Task<MediaUrlResponse> GetMediaUrlAsync(string mediaId, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// To retrieve your media’s URL, make a GET call to /{{Media-ID}}?phone_number_id=<PHONE_NUMBER_ID>. Later, you can use this URL to download the media file.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>MediaUrlResponse</returns>
        MediaUrlResponse GetMediaUrl(string mediaId, bool isMediaOwnershipVerified = false,  CancellationToken cancellationToken = default);

        /// <summary>
        /// To delete media, make a DELETE call to the ID of the media you want to delete.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> DeleteMediaAsync(string mediaId, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// To delete media, make a DELETE call to the ID of the media you want to delete.
        /// </summary>
        /// <param name="mediaId">ID for the media to send a media message or media template message to your customers.</param>
        /// <param name="isMediaOwnershipVerified">Verify the media ownership using PHONE_NUMBER_ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse DeleteMedia(string mediaId, bool isMediaOwnershipVerified = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// To download media uploaded from whatsapp
        /// </summary>
        /// <param name="mediaUrl">The URL generated from whatsapp cloud api</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>byte[]</returns>
        Task<byte[]> DownloadMediaAsync(string mediaUrl, CancellationToken cancellationToken = default);

        /// <summary>
        /// To download media uploaded from whatsapp
        /// </summary>
        /// <param name="mediaUrl">The URL generated from whatsapp cloud api</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>byte[]</returns>
        byte[] DownloadMedia(string mediaUrl, CancellationToken cancellationToken = default);
        #endregion

        #region Phone Numbers functions
        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        Task<PhoneNumberResponse> GetWhatsAppBusinessAccountPhoneNumberAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberResponse</returns>
        PhoneNumberResponse GetWhatsAppBusinessAccountPhoneNumber(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        Task<PhoneNumberByIdResponse> GetWhatsAppBusinessAccountPhoneNumberByIdAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>PhoneNumberByIdResponse</returns>
        PhoneNumberByIdResponse GetWhatsAppBusinessAccountPhoneNumberById(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified below.
        /// To verify a phone number using Graph API, make a POST request to {{PHONE_NUMBER_ID
        /// }}/ request_code.In your call, include your chosen verification method and locale. You need to authenticate yourself using { { User - Access - Token} } (This is automatically done for you in the Request Verification Code request).
        /// </summary>
        /// <param name="requestVerification">RequestVerificationCode object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        Task<VerificationResponse> RequestVerificationCodeAsync(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default);

        /// <summary>
        /// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified below.
        /// To verify a phone number using Graph API, make a POST request to {{PHONE_NUMBER_ID
        /// }}/ request_code.In your call, include your chosen verification method and locale. You need to authenticate yourself using { { User - Access - Token} } (This is automatically done for you in the Request Verification Code request).
        /// </summary>
        /// <param name="requestVerification">RequestVerificationCode object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        VerificationResponse RequestVerificationCode(RequestVerificationCode requestVerification, CancellationToken cancellationToken = default);

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        Task<VerificationResponse> VerifyCodeAsync(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you. To verify this code, make a POST request to {{PHONE_NUMBER_ID}}/verify_code that includes the code as a parameter.
        /// </summary>
        /// <param name="verifyCodeRequest">VerifyCodeRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>VerificationResponse</returns>
        VerificationResponse VerifyCode(VerifyCodeRequest verifyCodeRequest, CancellationToken cancellationToken = default);
        #endregion

        #region Registration functions
        /// <summary>
        /// To register your phone, make a POST call to /{{Phone-Number-ID}}/register
        /// </summary>
        /// <param name="registerPhoneRequest">RegisterPhoneRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> RegisterWhatsAppBusinessPhoneNumberAsync(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// To register your phone, make a POST call to /{{Phone-Number-ID}}/register
        /// </summary>
        /// <param name="registerPhoneRequest">RegisterPhoneRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse RegisterWhatsAppBusinessPhoneNumber(RegisterPhoneRequest registerPhoneRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> DeRegisterWhatsAppBusinessPhoneNumberAsync(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// To deregister your phone, make a POST call to {{Phone-Number-ID}}/deregister. Deregister Phone removes a previously registered phone. You can always re-register your phone using by repeating the registration process.
        /// </summary>
        /// <param name="whatsAppBusinessPhoneNumberId">ID for the phone number connected to the WhatsApp Business API. You can get this with a Get Phone Number ID request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse DeRegisterWhatsAppBusinessPhoneNumber(string whatsAppBusinessPhoneNumberId, CancellationToken cancellationToken = default);
        #endregion

        #region Send Messages functions
        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendAudioAttachmentMessageByIdAsync(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Audio Message using Media Id
        /// </summary>
        /// <param name="audioMessage">Audio Message Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendAudioAttachmentMessageById(AudioMessageByIdRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendAudioAttachmentMessageByUrlAsync(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Audio Message using hyperlink
        /// </summary>
        /// <param name="audioMessage">AudioMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendAudioAttachmentMessageByUrl(AudioMessageByUrlRequest audioMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendContactAttachmentMessageAsync(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Contact Message
        /// </summary>
        /// <param name="contactMessage">ContactMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendContactAttachmentMessage(ContactMessageRequest contactMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendDocumentAttachmentMessageByIdAsync(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Message using Media Id
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendDocumentAttachmentMessageById(DocumentMessageByIdRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendDocumentAttachmentMessageByUrlAsync(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Message using hyperlink
        /// </summary>
        /// <param name="documentMessage">DocumentMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendDocumentAttachmentMessageByUrl(DocumentMessageByUrlRequest documentMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Template Message
        /// </summary>
        /// <param name="documentTemplateMessageRequest">DocumentTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendDocumentAttachmentTemplateMessageAsync(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Document Template Message
        /// </summary>
        /// <param name="documentTemplateMessageRequest">DocumentTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendDocumentAttachmentTemplateMessage(DocumentTemplateMessageRequest documentTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendImageAttachmentMessageByIdAsync(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Message using Media Id
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendImageAttachmentMessageById(ImageMessageByIdRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendImageAttachmentMessageByUrlAsync(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Message using hyperlink
        /// </summary>
        /// <param name="imageMessage">ImageMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendImageAttachmentMessageByUrl(ImageMessageByUrlRequest imageMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendImageAttachmentTemplateMessageAsync(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Image Template Message
        /// </summary>
        /// <param name="imageTemplateMessageRequest">ImageTemplateMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendImageAttachmentTemplateMessage(ImageTemplateMessageRequest imageTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendInteractiveListMessageAsync(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive List Message
        /// </summary>
        /// <param name="interactiveListMessage">InteractiveListMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendInteractiveListMessage(InteractiveListMessageRequest interactiveListMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendInteractiveReplyButtonMessageAsync(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive Reply Button Message
        /// </summary>
        /// <param name="interactiveReplyButtonMessage">InteractiveReplyButtonMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendInteractiveReplyButtonMessage(InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive CTA Button Message
        /// </summary>
        /// <param name="interactiveCTAButtonMessageRequest">InteractiveCTAButtonMessageRequest Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendInteractiveCTAButtonMessageAsync(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive CTA Button Message
        /// </summary>
        /// <param name="interactiveCTAButtonMessageRequest">InteractiveCTAButtonMessageRequest Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendInteractiveCTAButtonMessage(InteractiveCTAButtonMessageRequest interactiveCTAButtonMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendInteractiveTemplateMessageAsync(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Interactive Template Message
        /// </summary>
        /// <param name="interactiveTemplateMessageRequest">InteractiveTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendInteractiveTemplateMessage(InteractiveTemplateMessageRequest interactiveTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendLocationMessageAsync(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Location Message
        /// </summary>
        /// <param name="locationMessageRequest">LocationMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendLocationMessage(LocationMessageRequest locationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

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
		Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

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
		MarkMessageResponse MarkMessageAsRead(MarkMessageRequest markMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send reaction message reply
        /// </summary>
        /// <param name="reactionMessageReply">ReactionMessageReply Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendReactionMessageReply(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send reaction message reply
        /// </summary>
        /// <param name="reactionMessageReply">ReactionMessageReply Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendReactionMessageReplyAsync(ReactionMessageReplyRequest reactionMessageReply, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendStickerMessageByIdAsync(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Sticker Message by Media Id
        /// </summary>
        /// <param name="stickerMessage">StickerMessage Object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendStickerMessageById(StickerMessageByIdRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendStickerMessageByUrlAsync(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Sticker Message by hyperlink
        /// </summary>
        /// <param name="stickerMessage">StickerMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendStickerMessageByUrl(StickerMessageByUrlRequest stickerMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendTextMessageAsync(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Text Message
        /// </summary>
        /// <param name="textMessage">TextMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendTextMessage(TextMessageRequest textMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendTextMessageTemplateAsync(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Template Text Message
        /// </summary>
        /// <param name="textTemplateMessageRequest">TextTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendTextMessageTemplate(TextTemplateMessageRequest textTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendVideoAttachmentMessageByIdAsync(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message by Template
        /// </summary>
        /// <param name="videoTemplateMessageRequest">VideoTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendVideoAttachmentTemplateMessage(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message by Template
        /// </summary>
        /// <param name="videoTemplateMessageRequest">VideoTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendVideoAttachmentTemplateMessageAsync(VideoTemplateMessageRequest videoTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message by Media Id
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendVideoAttachmentMessageById(VideoMessageByIdRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendVideoAttachmentMessageByUrlAsync(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Video Message using hyperlink
        /// </summary>
        /// <param name="videoMessage">VideoMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendVideoAttachmentMessageByUrl(VideoMessageByUrlRequest videoMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendSingleProductMessageAsync(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To send a single product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="singleProductMessage">SingleProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendSingleProductMessage(SingleProductMessageRequest singleProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendMultipleProductMessageAsync(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To send a multi-product message, make a POST call to the /v14.0/phone_number/messages endpoint.
        /// This request uses an interactive object and parameter type should be set to interactive.The interactive parameter must also be set to the interactive object associated with the single product message.
        /// </summary>
        /// <param name="multiProductMessage">MultiProductMessage object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendMultipleProductMessage(MultiProductMessageRequest multiProductMessage, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Authentication Template Message
		/// </summary>
		/// <param name="authenticationTemplateMessageRequest">AuthenticationTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		Task<WhatsAppResponse> SendAuthenticationMessageTemplateAsync(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Authentication Template Message
		/// </summary>
		/// <param name="authenticationTemplateMessageRequest">AuthenticationTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendAuthenticationMessageTemplate(AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send MPM Template Message
		/// </summary>
		/// <param name="multiProductTemplateMessageRequest">MultiProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		Task<WhatsAppResponse> SendMPMTemplateAsync(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send MPM Template Message
		/// </summary>
		/// <param name="multiProductTemplateMessageRequest">MultiProductTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendMPMTemplate(MultiProductTemplateMessageRequest multiProductTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Catalog Template Message
        /// </summary>
        /// <param name="catalogTemplateMessageRequest">CatalogTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendCatalogMessageTemplateAsync(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Catalog Template Message
		/// </summary>
		/// <param name="catalogTemplateMessageRequest">CatalogTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendCatalogMessageTemplate(CatalogTemplateMessageRequest catalogTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Catalog Message
        /// </summary>
        /// <param name="catalogMessageRequest">CatalogMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendCatalogMessageAsync(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Catalog Message
		/// </summary>
		/// <param name="catalogMessageRequest">CatalogMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendCatalogMessage(CatalogMessageRequest catalogMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Carousel Template Message
        /// </summary>
        /// <param name="carouselTemplateMessageRequest">CarouselTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WHatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendCarouselMessageTemplateAsync(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Carousel Template Message
		/// </summary>
		/// <param name="carouselTemplateMessageRequest">CarouselTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WHatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendCarouselMessageTemplate(CarouselTemplateMessageRequest carouselTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Coupon Code Template Message
        /// </summary>
        /// <param name="couponCodeTemplateMessageRequest">CouponCodeTemplateMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendCouponCodeMessageTemplateAsync(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Coupon Code Template Message
		/// </summary>
		/// <param name="couponCodeTemplateMessageRequest">CouponCodeTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendCouponCodeMessageTemplate(CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Limited Time Offer Template Message
		/// </summary>
		/// <param name="limitedTimeOfferTemplateMessageRequest">LimitedTimeOfferTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		Task<WhatsAppResponse> SendLimitedTimeOfferMessageTemplateAsync(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Limited Time Offer Template Message
		/// </summary>
		/// <param name="limitedTimeOfferTemplateMessageRequest">LimitedTimeOfferTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendLimitedTimeOfferMessageTemplate(LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Whatsapp Flow Messages
		/// </summary>
		/// <param name="flowMessageRequest">FlowMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>FlowMessageResponse</returns>
		Task<FlowMessageResponse> SendFlowMessageAsync(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Whatsapp Flow Messages
		/// </summary>
		/// <param name="flowMessageRequest">FlowMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>FlowMessageResponse</returns>
		FlowMessageResponse SendFlowMessage(FlowMessageRequest flowMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Whatsapp Flow Template Messages
		/// </summary>
		/// <param name="flowTemplateMessageRequest">FlowTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		Task<WhatsAppResponse> SendFlowMessageTemplateAsync(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Send Whatsapp Flow Template Messages
		/// </summary>
		/// <param name="flowTemplateMessageRequest">FlowTemplateMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendFlowMessageTemplate(FlowTemplateMessageRequest flowTemplateMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send generic or universal whatsapp message type that are not implemented in the library
        /// </summary>
        /// <param name="whatsAppMessageRequest">whatsAppMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        Task<WhatsAppResponse> SendGenericMessageAsync(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send generic or universal whatsapp message type that are not implemented in the library
        /// </summary>
        /// <param name="whatsAppMessageRequest">whatsAppMessageRequest object</param>
        /// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WhatsAppResponse</returns>
        WhatsAppResponse SendGenericMessage(object whatsAppMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Location request messages are free-form messages displaying body text and a send location button. When a WhatsApp user taps the button, a location sharing screen appears which the user can then use to share their location.
		/// </summary>
		/// <param name="interactiveLocationMessageRequest">interactiveLocationMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		Task<WhatsAppResponse> SendLocationRequestMessageAsync(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Location request messages are free-form messages displaying body text and a send location button. When a WhatsApp user taps the button, a location sharing screen appears which the user can then use to share their location.
		/// </summary>
		/// <param name="interactiveLocationMessageRequest">interactiveLocationMessageRequest object</param>
		/// <param name="cloudApiConfig">Custom WhatsAppBusinessCloudApiConfig</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>WhatsAppResponse</returns>
		WhatsAppResponse SendLocationRequestMessage(InteractiveLocationMessageRequest interactiveLocationMessageRequest, WhatsAppBusinessCloudApiConfig? cloudApiConfig = null, CancellationToken cancellationToken = default);
        #endregion

		#region Two step verification code function
		/// <summary>
		/// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
		/// You set up two-factor verification and register a phone number in the same API call.
		/// </summary>
		/// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>BaseSuccessResponse</returns>
		Task<BaseSuccessResponse> SetTwoStepVerificationAsync(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
        /// You set up two-factor verification and register a phone number in the same API call.
        /// </summary>
        /// <param name="twoStepVerificationRequest">TwoStepVerificationRequest object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse SetTwoStepVerificatiion(TwoStepVerificationRequest twoStepVerificationRequest, CancellationToken cancellationToken = default);
        #endregion

        #region WhatsApp Business Account functions
        /// <summary>
        /// Get Shared WhatsApp Business Account
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        Task<SharedWABAIDResponse> GetSharedWABAIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Shared WhatsApp Business Account
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAIDResponse</returns>
        SharedWABAIDResponse GetSharedWABAId(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        Task<SharedWABAResponse> GetSharedWABAListAsync(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Shared WhatsApp Business Account List
        /// </summary>
        /// <param name="businessId">Your Business' ID. Once you have your Phone-Number-ID, make a Get Business Profile request to get your Business' ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SharedWABAResponse</returns>
        SharedWABAResponse GetSharedWABAList(string businessId, CancellationToken cancellationToken = default);
        #endregion

        #region WABA subscription functions
        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> CreateWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse CreateWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        Task<SubscribedAppsResponse> GetWABASubscribedAppsAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// List all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>SubscribedAppsResponse</returns>
        SubscribedAppsResponse GetWABASubscribedApps(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> DeleteWABASubscriptionAsync(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        /// <param name="whatsAppBusinessAccountId">Your WhatsApp Business Account (WABA) ID.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse DeleteWABASubscription(string whatsAppBusinessAccountId, CancellationToken cancellationToken = default);
        #endregion

        #region Account Metrics
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
        AnalyticsResponse GetAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, CancellationToken cancellationToken = default);

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
        Task<AnalyticsResponse> GetAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? productTypes = null, List<string>? countryCodes = null, CancellationToken cancellationToken = default);

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
        ConversationAnalyticsResponse GetConversationAnalyticMetrics(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, CancellationToken cancellationToken = default);

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
        Task<ConversationAnalyticsResponse> GetConversationAnalyticMetricsAsync(string whatsAppBusinessAccountId, DateTime startDate, DateTime endDate, string granularity, List<string>? phoneNumbers = null, List<string>? metricTypes = null, List<string>? conversationTypes = null, List<string>? conversationDirections = null, List<string>? dimensions = null, CancellationToken cancellationToken = default);
        #endregion

        #region QR Code Message
        /// <summary>
        /// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="qrImageFormat"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        QRCodeMessageResponse CreateQRCodeMessage(string messageText, string qrImageFormat, CancellationToken cancellationToken = default);

        /// <summary>
        /// To create a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls endpoint with the prefilled_message parameter set to your message text and generate_qr_image parameter set to your preferred image format, either SVG or PNG.
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="qrImageFormat"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        Task<QRCodeMessageResponse> CreateQRCodeMessageAsync(string messageText, string qrImageFormat, CancellationToken cancellationToken = default);

        /// <summary>
        /// To get a list of all the QR codes messages for a business
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        QRCodeMessageFilterResponse GetQRCodeMessageList(CancellationToken cancellationToken = default);

        /// <summary>
        /// To get a list of all the QR codes messages for a business
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        Task<QRCodeMessageFilterResponse> GetQRCodeMessageListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// To get information about a specific QR code message
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        QRCodeMessageFilterResponse GetQRCodeMessageById(string qrCodeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// To get information about a specific QR code message
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageFilterResponse</returns>
        Task<QRCodeMessageFilterResponse> GetQRCodeMessageByIdAsync(string qrCodeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// To update a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls/{qr-code-id} endpoint and include the parameter you wish to update.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="messageText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        QRCodeMessageResponse UpdateQRCodeMessage(string qrCodeId, string messageText, CancellationToken cancellationToken = default);

        /// <summary>
        /// To update a QR code for a business, send a POST request to the /{phone-number-ID}/message_qrdls/{qr-code-id} endpoint and include the parameter you wish to update.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="messageText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>QRCodeMessageResponse</returns>
        Task<QRCodeMessageResponse> UpdateQRCodeMessageAsync(string qrCodeId, string messageText, CancellationToken cancellationToken = default);

        /// <summary>
        /// QR codes do not expire. You must delete a QR code in order to retire it.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>BaseSuccessResponse</returns>
        BaseSuccessResponse DeleteQRCodeMessage(string qrCodeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// QR codes do not expire. You must delete a QR code in order to retire it.
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>BaseSuccessResponse</returns>
        Task<BaseSuccessResponse> DeleteQRCodeMessageAsync(string qrCodeId, CancellationToken cancellationToken = default);
        #endregion
    }
}