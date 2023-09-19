using System;

namespace WhatsappBusiness.CloudApi
{
    public static class WhatsAppBusinessRequestEndpoint
    {
        /// <summary>
        /// WhatsApp Business Cloud API V15 BaseAddress
        /// </summary>
        public static Uri BaseAddress { get; private set; } = new Uri("https://graph.facebook.com/v18.0/");

        /// <summary>
        /// To register your phone to WhatsApp Business
        /// </summary>
        public static string RegisterPhone { get; private set; } = "{{Phone-Number-ID}}/register";

        /// <summary>
        /// To deregister your phone to WhatsApp Business
        /// </summary>
        public static string DeregisterPhone { get; private set; } = "{{Phone-Number-ID}}/deregister";

        /// <summary>
        /// To migrate account
        /// </summary>
        public static string MigrateAccount { get; private set; } = "{{Phone-Number-ID}}/register";

        /// <summary>
        /// The Resumable Upload series of requests allow you to upload Profile Pictures to Meta so you can receive a handle to update these pictures in the Business Profile API.
        /// </summary>
        public static string ResumableUploadCreateUploadSession { get; private set; } = "app/uploads/?file_length={{FILE_LENGTH}}&file_type={{FILE_TYPE}}&file_name={{FILE_NAME}}";

        /// <summary>
        /// To upload a profile picture to your business profile, make a POST call to the named endpoint v14.0/{{Upload-ID}}, where Upload-ID is the value you received from Resumable Upload - Create an Upload Session.
        /// </summary>
        public static string ResumableUploadFileData { get; private set; } = "{{Upload-ID}}";

        /// <summary>
        /// You can query the status of an upload session by making a GET call to an endpoint that is named based on the Upload-ID that was returned through the Resumable Upload - Create an Upload Session request.
        /// </summary>
        public static string ResumableUploadQueryFileUploadStatus { get; private set; } = "{{Upload-ID}}";

        /// <summary>
        /// Get WhatsApp Business Profile Account
        /// </summary>
        public static string GetBusinessProfileId { get; private set; } = "{{Phone-Number-ID}}/whatsapp_business_profile";

        /// <summary>
        /// Update WhatsApp Business Profile Account
        /// </summary>
        public static string UpdateBusinessProfileId { get; private set; } = "{{Phone-Number-ID}}/whatsapp_business_profile";

        /// <summary>
        /// All media files sent through this endpoint are encrypted and persist for 30 days.
        /// </summary>
        public static string UploadMedia { get; private set; } = "{{Phone-Number-ID}}/media";

        /// <summary>
        /// Download media file from the generated whatsapp media url
        /// </summary>
        public static string DownloadMedia { get; private set; } = "{{Media-URL}}";

        /// <summary>
        /// To retrieve your media’s URL for downloading.
        /// The URL is only valid for 5 minutes.
        /// </summary>
        public static string GetMediaUrl { get; private set; } = "{{Media-ID}}";

        /// <summary>
        /// To retrieve media URL with phone number id ownership
        /// </summary>
        public static string GetMediaUrlOwnership { get; private set; } = "{{Media-ID}}?phone_number_id={{PHONE_NUMBER_ID}}";

        /// <summary>
        /// Delete media
        /// </summary>
        public static string DeleteMedia { get; private set; } = "{{Media-ID}}";

        /// <summary>
        /// Delete Media with phone number id ownership
        /// </summary>
        public static string DeleteMediaOwnership { get; private set; } = "{{Media-ID}}/?phone_number_id={{PHONE_NUMBER_ID}}";

        /// <summary>
        /// Endpoint to send WhatsApp Messages
        /// </summary>
        public static string SendMessage { get; private set; } = "{{Phone-Number-ID}}/messages";

        /// <summary>
        /// Endpoint to send WhatsApp Messages
        /// </summary>
        public static string MarkMessageAsRead { get; private set; } = "{{Phone-Number-ID}}/messages";

        /// <summary>
        /// This API returns all phone numbers in a WhatsApp Business Account specified by the {{WABA-ID}} value. Get the id value for the phone number you want to use to send messages with WhatsApp Business Cloud API.
        /// </summary>
        public static string GetPhoneNumbers { get; private set; } = "{{WABA-ID}}/phone_numbers";

        /// <summary>
        /// When you query all the phone numbers for a WhatsApp Business Account, each phone number has an id. You can directly query for a phone number using this id
        /// </summary>
        public static string GetPhoneNumberById { get; private set; } = "{{Phone-Number-ID}}";

        /// <summary>
        /// You need to verify the phone number you want to use to send messages to your customers. Phone numbers must be verified through SMS/voice call. The verification process can be done through the Graph API calls specified
        /// </summary>
        public static string RequestVerificationCode { get; private set; } = "{{Phone-Number-ID}}/request_code";

        /// <summary>
        /// After you received a SMS or Voice request code from Request Verification Code, you need to verify the code that was sent to you.
        /// </summary>
        public static string VerifyCode { get; private set; } = "{{Phone-Number-ID}}/verify_code";

        /// <summary>
        /// You can use this endpoint to change two-step verification code associated with your account. After you change the verification code, future requests like changing the name, must use the new code.
        /// </summary>
        public static string SetTwoFactor { get; private set; } = "{{Phone-Number-ID}}";

        public static string GetSharedWABAID { get; private set; } = "debug_token";

        public static string GetListSharedWABA { get; private set; } = "{{Business-ID}}/client_whatsapp_business_accounts";

        /// <summary>
        /// Subscribe an app to a WhatsApp Business Account.
        /// </summary>
        public static string SubscribeAppToWABA { get; private set; } = "{{WABA-ID}}/subscribed_apps";

        /// <summary>
        /// You could also list all the current app subscriptions to a given WhatsApp Business Account.
        /// </summary>
        public static string GetSubscribedApps { get; private set; } = "{{WABA-ID}}/subscribed_apps";

        /// <summary>
        /// If you don’t want an application to receive webhooks for a given WhatsApp Business Account anymore you can delete the subscription.
        /// </summary>
        public static string DeleteSubscribedApps { get; private set; } = "{{WABA-ID}}/subscribed_apps";

        /// <summary>
        /// You can use the analytics and conversation_analytics fields to get metrics about messages and conversations associated with your WhatsApp Business Account (WABA). Specifically, you can get the number of messages sent and delivered as well as conversation and cost information for a given period.
        /// </summary>
        public static string AnalyticsAccountMetrics { get; private set; } = "{{WABA-ID}}?fields=analytics.start({{start-date}}).end({{end-date}}).granularity({{granularity}})";

        /// <summary>
        /// You can use the analytics and conversation_analytics fields to get metrics about messages and conversations associated with your WhatsApp Business Account (WABA). Specifically, you can get the number of messages sent and delivered as well as conversation and cost information for a given period.
        /// </summary>
        public static string ConversationAnalyticsAccountMetrics { get; private set; } = "{{WABA-ID}}?fields=conversation_analytics.start({{start-date}}).end({{end-date}}).granularity({{granularity}})";

        public static string CreateQRCodeMessage { get; private set; } = "{{Phone-Number-ID}}/message_qrdls?prefilled_message={{message-text}}&generate_qr_image={{image-format}}&access_token={{user-access-token}}";

        public static string GetQRCodeMessage { get; private set; } = "{{Phone-Number-ID}}/message_qrdls";

        public static string GetQRCodeMessageById { get; private set; } = "{{Phone-Number-ID}}/message_qrdls/{{qr-code-id}}";

        public static string UpdateQRCodeMessage { get; private set; } = "{{Phone-Number-ID}}/message_qrdls/{{qr-code-id}}?prefilled_message={{new-message-text}}&access_token={{user-access-token}}";

        public static string DeleteQRCodeMessage { get; private set; } = "{{Phone-Number-ID}}/message_qrdls/{{qr-code-id}}&access_token={{user-access-token}}";
    }
}
