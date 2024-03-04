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


	public enum enumMessageType
	{
		Audio,
		Doc,
		Image,
		Text,
		Video
	}

	public record MessageType(enumMessageType Type, bool Template, bool Att, bool Cap);

	public record SendTextPayload
    {
        public required string ToNum { get; set; }
        public string Message { get; set; } = "Hello";
        public bool PreviewUrl { get; set; } = false;
	}

	public record WhatsAppMedia
	{
		public string Type { get; set; }
		public string URL { get; set; }
		public string ID { get; set; }
		public string Caption { get; set; }		
	}

    public record WhatsappTemplate
    {
		public string Name { get; set; }
		public List<string> Params { get; set; }
	}

    /// <summary>
    /// This Payload cater for a message
    /// 1. Text Only
    /// 2. Media
    /// 3. Templates
    /// </summary>
	public record SendWhatsAppPayload
    {
        public SendTextPayload SendText { get; set; }
		public enumMessageType MessageType { get; set; }
		public WhatsAppMedia Media { get; set; }
        public WhatsappTemplate Template { get; set; }
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

		/// <summary>
		/// This is to prep the phone number to be in the correct length and form for whatsapp.
        /// 1. If the Phone number is Null return -1
        /// 2. Number must start with the country code assume South Africa (27)
        /// 3. For South Africa the number must be minimum 9 digits without the country code and any leading zeros if less return -1
        /// 4. If the number is more than 9 digits then assume the number is correct
		/// </summary>
		/// <param name="phoneNumber"></param>
		/// <returns>String: a formated number for WhatsApp</returns>
		[HttpPost]
        [Route("[action]")]
        public string PrepNumber(string phoneNumber)
        {
			// Step 1: Strip out any character that is not a digit
			string digitsOnly = phoneNumber?.Where(char.IsDigit).Any() == true ? new string(phoneNumber.Where(char.IsDigit).ToArray()) : "-1";

			// Step 2: If the first character is a 0, then Remove it
			if (digitsOnly.Length > 9 && digitsOnly[0] == '0')
            { // Remove any leading zero
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

		/// <summary>
		/// Text With/Without Template With/Without Params
		/// This is made to send Text Messages With or Without a Template
		/// If with a Template then With or Without Parameters
		/// </summary>
		/// <param name="payload"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("[action]")]
		public async Task<ActionResult<WhatsAppResponse>> SendWhatsApp_TextAsync(SendWhatsAppPayload payload)
		{
            try
            {				
                if (payload.Template == null)
                {// Simple text only
                    TextMessageRequest textMsgPayload = new();
                    textMsgPayload.To = payload.SendText.ToNum;
                    textMsgPayload.Text = new WhatsAppText();                    
					textMsgPayload.Text.Body = payload.SendText.Message;
					textMsgPayload.Text.PreviewUrl = payload.SendText.PreviewUrl;

					var result = await _whatsAppBusinessClient.SendTextMessageAsync(textMsgPayload);
					string WAMIds = GetWAMId(result);
					_logger.LogInformation($"Sent Message: WAMID = '{WAMIds}'");

					return result;
				}
                else
                {// Text Template
                    TextTemplateMessageRequest textMsgPayload = new TextTemplateMessageRequest();
			        textMsgPayload.To = payload.SendText.ToNum;
			        textMsgPayload.Template = new TextMessageTemplate();
			        textMsgPayload.Template.Name = payload.Template.Name;
			        textMsgPayload.Template.Language = new TextMessageLanguage();
			        textMsgPayload.Template.Language.Code = LanguageCode.English_US;

                    if(payload.Template.Params != null)
                    {// Text Template with params
                        // For Text WhatsappTemplate message with parameters supported component type is body only				
			            textMsgPayload.Template.Components = new List<TextMessageComponent>();

			            var parameters = new List<TextMessageParameter>();

			            foreach (var txt in payload.Template.Params)
			            {
				            var param = new TextMessageParameter()
				            {
					            Type = "text",
					            Text = txt
				            };
				            parameters.Add(param);
			            }

			            textMsgPayload.Template.Components.Add(new TextMessageComponent()
			            {
				            Type = "body",
				            Parameters = parameters
			            });
                    }
                    var result = await _whatsAppBusinessClient.SendTextMessageTemplateAsync(textMsgPayload);
					string WAMIds = GetWAMId(result);
					_logger.LogInformation($"Sent Message: WAMID = '{WAMIds}'");
					return result;
                }   
            }
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return Ok(-1);
			}
		}

		/// <summary>
		/// This is to send NON WhatsappTemplate messages for:
		///     Audio, Document, Image, Sticker, Video
		/// </summary>
		/// <param name="payload"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<WhatsAppResponse>> SendWhatsApp_MediaAsync(SendWhatsAppPayload payload)
        {
            try
            {
                WhatsAppResponse results = null;
                switch (payload.MessageType)
                {
                    case enumMessageType.Audio:
                        if (!string.IsNullOrWhiteSpace(payload.Media.ID))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            AudioMessageByIdRequest audioMessage = new AudioMessageByIdRequest();
                            audioMessage.To = payload.SendText.ToNum;
                            audioMessage.Audio = new MediaAudio();
                            audioMessage.Audio.Id = payload.Media.ID;
                            
                            results = await _whatsAppBusinessClient.SendAudioAttachmentMessageByIdAsync(audioMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.URL))
                        {
                            AudioMessageByUrlRequest audioMessage = new AudioMessageByUrlRequest();
                            audioMessage.To = payload.SendText.ToNum;
                            audioMessage.Audio = new MediaAudioUrl();
                            audioMessage.Audio.Link = payload.Media.URL;

                            results = await _whatsAppBusinessClient.SendAudioAttachmentMessageByUrlAsync(audioMessage);
                        }
                        break;

                    case enumMessageType.Doc:
                        if (!string.IsNullOrWhiteSpace(payload.Media.ID))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            DocumentMessageByIdRequest documentMessage = new DocumentMessageByIdRequest();
                            documentMessage.To = payload.SendText.ToNum;
                            documentMessage.Document = new MediaDocument();
                            documentMessage.Document.Id = payload.Media.ID;
                            documentMessage.Document.Caption = payload.Media.Caption;

                            results = await _whatsAppBusinessClient.SendDocumentAttachmentMessageByIdAsync(documentMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.URL))
                        {
                            DocumentMessageByUrlRequest documentMessage = new DocumentMessageByUrlRequest();
                            documentMessage.To = payload.SendText.ToNum;
                            documentMessage.Document = new MediaDocumentUrl();
                            documentMessage.Document.Link = payload.Media.URL;
                            documentMessage.Document.Caption = payload.Media.Caption;

                            results = await _whatsAppBusinessClient.SendDocumentAttachmentMessageByUrlAsync(documentMessage);
                        }
                        break;

                    case enumMessageType.Image:
                        if (!string.IsNullOrWhiteSpace(payload.Media.ID))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            ImageMessageByIdRequest imageMessage = new ImageMessageByIdRequest();
                            imageMessage.To = payload.SendText.ToNum;
                            imageMessage.Image = new MediaImage();
                            imageMessage.Image.Id = payload.Media.ID;
                            imageMessage.Image.Caption = payload.Media.Caption;

							results = await _whatsAppBusinessClient.SendImageAttachmentMessageByIdAsync(imageMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.URL))
                        {
                            ImageMessageByUrlRequest imageMessage = new ImageMessageByUrlRequest();
                            imageMessage.To = payload.SendText.ToNum;
                            imageMessage.Image = new MediaImageUrl();
                            imageMessage.Image.Link = payload.Media.URL;
                            imageMessage.Image.Caption = payload.Media.Caption;

							results = await _whatsAppBusinessClient.SendImageAttachmentMessageByUrlAsync(imageMessage);
                        }
                        break;

                    //case "STICKER":
                    //    if (!string.IsNullOrWhiteSpace(payload.Media.ID))
                    //    {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                    //        StickerMessageByIdRequest stickerMessage = new StickerMessageByIdRequest();
                    //        stickerMessage.To = payload.SendText.ToNum;
                    //        stickerMessage.Sticker = new MediaSticker();
                    //        stickerMessage.Sticker.Id = payload.Media.ID;

                    //        results = await _whatsAppBusinessClient.SendStickerMessageByIdAsync(stickerMessage);
                    //    }
                    //    else //if (!string.IsNullOrWhiteSpace(payload.URL))
                    //    {
                    //        StickerMessageByUrlRequest stickerMessage = new StickerMessageByUrlRequest();
                    //        stickerMessage.To = payload.SendText.ToNum;
                    //        stickerMessage.Sticker = new MediaStickerUrl();
                    //        stickerMessage.Sticker.Link = payload.Media.URL;

                    //        results = await _whatsAppBusinessClient.SendStickerMessageByUrlAsync(stickerMessage);
                    //    }
                    //    break;

                    case enumMessageType.Video:
                        if (!string.IsNullOrWhiteSpace(payload.Media.ID))
                        {  // Usaing IDs is much better, Upload the file to WhatsApp and then use the ID returned
                            VideoMessageByIdRequest videoMessage = new VideoMessageByIdRequest();
                            videoMessage.To = payload.SendText.ToNum;
                            videoMessage.Video = new MediaVideo();
                            videoMessage.Video.Id = payload.Media.ID;
                            videoMessage.Video.Caption = payload.Media.Caption;

							results = await _whatsAppBusinessClient.SendVideoAttachmentMessageByIdAsync(videoMessage);
                        }
                        else //if (!string.IsNullOrWhiteSpace(payload.URL))
                        {
                            VideoMessageByUrlRequest videoMessage = new VideoMessageByUrlRequest();
                            videoMessage.To = payload.SendText.ToNum;
                            videoMessage.Video = new MediaVideoUrl();
                            videoMessage.Video.Link = payload.Media.URL;
                            videoMessage.Video.Caption = payload.Media.Caption;

							results = await _whatsAppBusinessClient.SendVideoAttachmentMessageByUrlAsync(videoMessage);
                        }
                        break;
                }               
                return results;
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(-1);      // RedirectToAction(nameof(SendWhatsApp_MediaAsync)).WithDanger("Error", ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<WhatsAppResponse>> SendWhatsApp_TemplateImage_ParameterAsync(SendWhatsAppPayload payload)
        {
            // Tested with facebook predefined template name: sample_movie_ticket_confirmation
            ImageTemplateMessageRequest imageTemplateMessage = new ImageTemplateMessageRequest();
            imageTemplateMessage.To = payload.SendText.ToNum;
            imageTemplateMessage.Template = new ImageMessageTemplate();
            imageTemplateMessage.Template.Name = payload.Template.Name;
            imageTemplateMessage.Template.Language = new ImageMessageLanguage();
            imageTemplateMessage.Template.Language.Code = LanguageCode.English_US;

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
                            Id = !string.IsNullOrEmpty(payload.Media.ID) ? payload.Media.ID : null,
							Link = string.IsNullOrEmpty(payload.Media.ID) ? payload.Media.URL : null,
                            //Caption = !string.IsNullOrEmpty(payload.Caption) ? payload.Caption : null
                        }
					}
                },
            });

            if (payload.Template.Params != null)
            {
                // Loop and Compile Body Params
                var bodyParams = new List<ImageMessageParameter>();

                foreach (var txt in payload.Template.Params)
                {
                    var param = new ImageMessageParameter()
                    {
                        Type = "text",
                        Text = txt
                    };
                    bodyParams.Add(param);
                }

                // Add the Body Params
                imageTemplateMessage.Template.Components.Add(new ImageMessageComponent()
                {
                    Type = "body",
                    Parameters = bodyParams
                });
            }

            var results = await _whatsAppBusinessClient.SendImageAttachmentTemplateMessageAsync(imageTemplateMessage);
            return results;
        }

        [HttpPost]
        [Route("[action]")]		
		public async Task<ActionResult<WhatsAppResponse>> SendWhatsApp_TemplateVideo_ParameterAsync(SendWhatsAppPayload payload)
		{
            // Senbd a Video WhatsappTemplate with Parameters

            VideoTemplateMessageRequest videoTemplateMessage = new();
            videoTemplateMessage.To = payload.SendText.ToNum;            
            videoTemplateMessage.Template = new();
            videoTemplateMessage.Template.Name = payload.Template.Name;
            videoTemplateMessage.Template.Language = new();
            videoTemplateMessage.Template.Language.Code = LanguageCode.English_US;
            
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
                            Id = !string.IsNullOrEmpty(payload.Media.ID) ? payload.Media.ID : null,
                            Link = string.IsNullOrEmpty(payload.Media.ID) ? payload.Media.URL : null,
                            //Caption = !string.IsNullOrEmpty(payload.Caption) ? payload.Caption : null
                        }
                    }
                },
            });

            if (payload.Template.Params != null)
            { // There are Params, Loop and Compile Body Params
                var bodyParams = new List<VideoMessageParameter>();

                foreach (var txt in payload.Template.Params)
                {
                    var param = new VideoMessageParameter()
                    {
                        Type = "text",
                        Text = txt
                    };
                    bodyParams.Add(param);
                }

                // Add the Body Params
                videoTemplateMessage.Template.Components.Add(new VideoMessageComponent()
                {
                    Type = "body",
                    Parameters = bodyParams
                });
            }

            var results = await _whatsAppBusinessClient.SendVideoAttachmentTemplateMessageAsync(videoTemplateMessage);

			return results;
        }
	}
}
