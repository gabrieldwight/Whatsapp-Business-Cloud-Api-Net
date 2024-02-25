using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.Webhook;
using WhatsAppBusinessCloudAPI.Web.Extensions.Alerts;
using WhatsAppBusinessCloudAPI.Web.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
    public record FileUploadPayload
    {
        public string fileName { get; set; }
        public string uploadType { get; set; }
    }

    public record SendTextPayload
    {
        public required string PhoneNumber { get; set; }
        public string Message { get; set; } = "Hello";
        public bool PreviewUrl { get; set; } = false;
	}
    
    public record SendMediaMessagePayload
    {
        public string PhoneNumber { get; set; }
        public string MediaType { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string? MediaLink { get; set; }
        public string? MediaId { get; set; }
    }
   
    public record SendMediaURLPayload
    {
        public string PhoneNumber { get; set; }
        public string mediaURL { get; set; }
        public string Message { get; set; }
    }

    public record SendTemplate_text_ParameterPayload
    {
        public string PhoneNumber { get; set; }
        public string TemplateName { get; set; }
        public List<string> TemplateParams { get; set; }

    }

    public record SendTemplate_media_ParameterPayload
    {
        public string phoneNumber { get; set; }
        public string templateName { get; set; }
        public string mediaURL { get; set; }
        public string mediaID { get; set; }
        public string mediaText { get; set; }
        public List<string> templateParams { get; set; }

    }

    public class SendMessageController : Controller
    {
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HomeController> _logger;        
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        
        public SendMessageController(ILogger<HomeController> logger, IWhatsAppBusinessClient whatsAppBusinessClient, IWebHostEnvironment environment)
        {
			_logger = logger;
			_whatsAppBusinessClient = whatsAppBusinessClient;
			_environment = environment;
		}



		public string GetWAMId(WhatsAppResponse whatsAppResponse)
		{
			// This is to try and get the WAMID

			StringBuilder msgIDsBuilder = new StringBuilder();

			foreach (Message msg in whatsAppResponse.Messages)
			{
				msgIDsBuilder.Append(msg.Id);
				msgIDsBuilder.Append(", ");
			}

			return msgIDsBuilder.ToString();
		}

        [HttpPost]
        [Route("[action]")]
        public string PrepNumber(string phoneNumber)
        {
            // This is to prep the phone number to be in the correct length and form for whatsapp
            // Step 1: Strip out any character that is not a digit
            string digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Step 2: If the first character is a 0, then Remove it
            if (digitsOnly.Length > 0 && digitsOnly[0] == '0')
            {
                digitsOnly = digitsOnly.Remove(0, 1);
            }

            // Test the length of the number with a switch
            return digitsOnly.Length switch
            {
                < 9 => "-1",// Return -1 if the length is less than 9
                9 => "27" + digitsOnly,// Add 27 in front of the number if the length is 9
                _ => digitsOnly,// Return the number if the length is more than 9
            };
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult<WhatsAppResponse>> SendTextAsync(SendTextPayload payload)
        {
            TextMessageRequest textMessageRequest = new TextMessageRequest();
            textMessageRequest.To = payload.PhoneNumber;
            textMessageRequest.Text = new WhatsAppText();
            textMessageRequest.Text.Body = payload.Message;
            textMessageRequest.Text.PreviewUrl = payload.PreviewUrl;

            var results = await _whatsAppBusinessClient.SendTextMessageAsync(textMessageRequest);
			string WAMIds = GetWAMId(results);

			// Process or perform operations with the record fields
			Console.WriteLine($"List of WAMIds: '{WAMIds}'");

			return results;
        }
		
		/// <summary>
		/// This is to send NON Template messages for:
		///     Audio, Document, Image, Sticker, Video
		/// </summary>
		/// <param name="payload"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<WhatsAppResponse>> SendWhatsAppMediaMessage(SendMediaMessagePayload payload)
        {
            try
            {
                WhatsAppResponse results = null;
                switch (payload.MessageType.ToUpper())
                {
                    case "AUDIO":
                        if (!string.IsNullOrWhiteSpace(payload.MediaId))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            AudioMessageByIdRequest audioMessage = new AudioMessageByIdRequest();
                            audioMessage.To = payload.PhoneNumber;
                            audioMessage.Audio = new MediaAudio();
                            audioMessage.Audio.Id = payload.MediaId;

                            results = await _whatsAppBusinessClient.SendAudioAttachmentMessageByIdAsync(audioMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.MediaLink))
                        {
                            AudioMessageByUrlRequest audioMessage = new AudioMessageByUrlRequest();
                            audioMessage.To = payload.PhoneNumber;
                            audioMessage.Audio = new MediaAudioUrl();
                            audioMessage.Audio.Link = payload.MediaLink;

                            results = await _whatsAppBusinessClient.SendAudioAttachmentMessageByUrlAsync(audioMessage);
                        }
                        break;

                    case "DOCUMENT":
                        if (!string.IsNullOrWhiteSpace(payload.MediaId))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            DocumentMessageByIdRequest documentMessage = new DocumentMessageByIdRequest();
                            documentMessage.To = payload.PhoneNumber;
                            documentMessage.Document = new MediaDocument();
                            documentMessage.Document.Id = payload.MediaId;
                            documentMessage.Document.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendDocumentAttachmentMessageByIdAsync(documentMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.MediaLink))
                        {
                            DocumentMessageByUrlRequest documentMessage = new DocumentMessageByUrlRequest();
                            documentMessage.To = payload.PhoneNumber;
                            documentMessage.Document = new MediaDocumentUrl();
                            documentMessage.Document.Link = payload.MediaLink;
                            documentMessage.Document.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendDocumentAttachmentMessageByUrlAsync(documentMessage);
                        }
                        break;

                    case "IMAGE":
                        if (!string.IsNullOrWhiteSpace(payload.MediaId))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            ImageMessageByIdRequest imageMessage = new ImageMessageByIdRequest();
                            imageMessage.To = payload.PhoneNumber;
                            imageMessage.Image = new MediaImage();
                            imageMessage.Image.Id = payload.MediaId;
                            imageMessage.Image.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendImageAttachmentMessageByIdAsync(imageMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.MediaLink))
                        {
                            ImageMessageByUrlRequest imageMessage = new ImageMessageByUrlRequest();
                            imageMessage.To = payload.PhoneNumber;
                            imageMessage.Image = new MediaImageUrl();
                            imageMessage.Image.Link = payload.MediaLink;
                            imageMessage.Image.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendImageAttachmentMessageByUrlAsync(imageMessage);
                        }
                        break;

                    case "STICKER":
                        if (!string.IsNullOrWhiteSpace(payload.MediaId))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            StickerMessageByIdRequest stickerMessage = new StickerMessageByIdRequest();
                            stickerMessage.To = payload.PhoneNumber;
                            stickerMessage.Sticker = new MediaSticker();
                            stickerMessage.Sticker.Id = payload.MediaId;

                            results = await _whatsAppBusinessClient.SendStickerMessageByIdAsync(stickerMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.MediaLink))
                        {
                            StickerMessageByUrlRequest stickerMessage = new StickerMessageByUrlRequest();
                            stickerMessage.To = payload.PhoneNumber;
                            stickerMessage.Sticker = new MediaStickerUrl();
                            stickerMessage.Sticker.Link = payload.MediaLink;

                            results = await _whatsAppBusinessClient.SendStickerMessageByUrlAsync(stickerMessage);
                        }
                        break;

                    case "VIDEO":
                        if (!string.IsNullOrWhiteSpace(payload.MediaId))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            VideoMessageByIdRequest videoMessage = new VideoMessageByIdRequest();
                            videoMessage.To = payload.PhoneNumber;
                            videoMessage.Video = new MediaVideo();
                            videoMessage.Video.Id = payload.MediaId;
                            videoMessage.Video.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendVideoAttachmentMessageByIdAsync(videoMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.MediaLink))
                        {
                            VideoMessageByUrlRequest videoMessage = new VideoMessageByUrlRequest();
                            videoMessage.To = payload.PhoneNumber;
                            videoMessage.Video = new MediaVideoUrl();
                            videoMessage.Video.Link = payload.MediaLink;
                            videoMessage.Video.Caption = payload.Message;

                            results = await _whatsAppBusinessClient.SendVideoAttachmentMessageByUrlAsync(videoMessage);
                        }
                        break;
                }               
                return results;
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(-1);      // RedirectToAction(nameof(SendWhatsAppMediaMessage)).WithDanger("Error", ex.Message);
            }
        }

		[HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<WhatsAppResponse>> SendTemplate_text_ParameterAsync(SendTemplate_text_ParameterPayload payload)
        {
            // For Text Template message with parameters supported component type is body only
            TextTemplateMessageRequest textTemplateMessage = new TextTemplateMessageRequest();
            textTemplateMessage.To = payload.PhoneNumber;
            textTemplateMessage.Template = new TextMessageTemplate();
            textTemplateMessage.Template.Name = payload.TemplateName;
            textTemplateMessage.Template.Language = new TextMessageLanguage();
            textTemplateMessage.Template.Language.Code = LanguageCode.English_US;
            textTemplateMessage.Template.Components = new List<TextMessageComponent>();

            var parameters = new List<TextMessageParameter>();

            foreach (var txt in payload.TemplateParams)
            {
                var param = new TextMessageParameter()
                {
                    Type = "text",
                    Text = txt
                };
                parameters.Add(param);
            }

            textTemplateMessage.Template.Components.Add(new TextMessageComponent()
            {
                Type = "body",
                Parameters = parameters
            });

            var results = await _whatsAppBusinessClient.SendTextMessageTemplateAsync(textTemplateMessage);
            return results;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<WhatsAppResponse>> SendTemplate_image_ParameterAsync(SendTemplate_media_ParameterPayload payload)
        {
            // Tested with facebook predefined template name: sample_movie_ticket_confirmation
            ImageTemplateMessageRequest imageTemplateMessage = new ImageTemplateMessageRequest();
            imageTemplateMessage.To = payload.phoneNumber;
            imageTemplateMessage.Template = new ImageMessageTemplate();
            imageTemplateMessage.Template.Name = payload.templateName;
            imageTemplateMessage.Template.Language = new ImageMessageLanguage();
            imageTemplateMessage.Template.Language.Code = LanguageCode.English_US;

            // Loop and Compile Body Params
            var bodyParams = new List<ImageMessageParameter>();

            foreach (var txt in payload.templateParams)
            {
                var param = new ImageMessageParameter()
                {
                    Type = "text",
                    Text = txt
                };
                bodyParams.Add(param);
            }

            imageTemplateMessage.Template.Components = new List<ImageMessageComponent>(); // Move this line here

            imageTemplateMessage.Template.Components.Add(new ImageMessageComponent()
            {
                Type = "header",
                Parameters = new List<ImageMessageParameter>()
            {
                new ImageMessageParameter()
                {
                    Type = "image",
                    Image = new WhatsappBusiness.CloudApi.Messages.Requests.Image()
                    {
                        Link = payload.mediaURL
                    }
                }
            },
            });

            // Add the Body Params
            imageTemplateMessage.Template.Components.Add(new ImageMessageComponent()
            {
                Type = "body",
                Parameters = bodyParams
            });

            var results = await _whatsAppBusinessClient.SendImageAttachmentTemplateMessageAsync(imageTemplateMessage);
            return results;
        }

        [HttpPost]
        [Route("[action]")]		
		public async Task<WhatsAppResponse> SendTemplate_video_ParameterAsync(SendTemplate_media_ParameterPayload payload)
		{
            // Senbd a Video Template with Parameters

            VideoTemplateMessageRequest videoTemplateMessage = new();
            videoTemplateMessage.To = payload.phoneNumber;
            videoTemplateMessage.Template = new();
            videoTemplateMessage.Template.Name = payload.templateName;
            videoTemplateMessage.Template.Language = new();
            videoTemplateMessage.Template.Language.Code = LanguageCode.English_US;

            // Loop and Compile Body Params
            var bodyParams = new List<VideoMessageParameter>();

            foreach (var txt in payload.templateParams)
            {
                var param = new VideoMessageParameter()
                {
                    Type = "text",
                    Text = txt
                };
                bodyParams.Add(param);
            }

            videoTemplateMessage.Template.Components = new List<VideoMessageComponent>();

            videoTemplateMessage.Template.Components.Add(new VideoMessageComponent()
            {
                Type = "header",
                Parameters = new List<VideoMessageParameter>()
                {
                    new VideoMessageParameter()
                    {
                        Type = "video",
                        Video = new WhatsappBusiness.CloudApi.Messages.Requests.Video()
                        {
                            //Id = payload.mediaID
                            //Link = payload.mediaURL // Link point where your document can be downloaded or retrieved by WhatsApp
                            Id = !string.IsNullOrEmpty(payload.mediaID) ? payload.mediaID : null,
                            Link = string.IsNullOrEmpty(payload.mediaID) ? payload.mediaURL : null,
                            //Caption = !string.IsNullOrEmpty(payload.mediaText) ? payload.mediaText : null
                        }
                    }
                },
            });

            // Add the Body Params
            videoTemplateMessage.Template.Components.Add(new VideoMessageComponent()
            {
                Type = "body",
                Parameters = bodyParams
            });

            var results = await _whatsAppBusinessClient.SendVideoAttachmentTemplateMessageAsync(videoTemplateMessage);

			return results;
        }

    }
}
