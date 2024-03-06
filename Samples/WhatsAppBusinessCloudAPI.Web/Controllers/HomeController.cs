using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Text;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.AccountMetrics;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsAppBusinessCloudAPI.Web.Extensions.Alerts;
using WhatsAppBusinessCloudAPI.Web.Models;
using WhatsAppBusinessCloudAPI.Web.ViewModel;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        private readonly IWebHostEnvironment _environment;
        private readonly SendMessageController _sendMessageController;

        public HomeController(ILogger<HomeController> logger, IWhatsAppBusinessClient whatsAppBusinessClient,
            IOptions<WhatsAppBusinessCloudApiConfig> whatsAppConfig, IWebHostEnvironment environment)
        {
            _logger = logger;
            _whatsAppBusinessClient = whatsAppBusinessClient;
            _whatsAppConfig = whatsAppConfig.Value;
            _environment = environment;

            _sendMessageController = new(_logger, _whatsAppBusinessClient, _environment);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendWhatsAppTextMessage()
        {
            return View();
        }

		public IActionResult SendWhatsAppMediaMessage()
        {
			SendMediaMessageViewModel sendMediaMessageViewModel = new SendMediaMessageViewModel
			{
				MediaType = new List<SelectListItem>()
	{
				new SelectListItem(){ Text = "Audio", Value = enumMessageType.Audio.ToString() },
				new SelectListItem(){ Text = "Document", Value = enumMessageType.Doc.ToString() },
				new SelectListItem(){ Text = "Image", Value = enumMessageType.Image.ToString() },
				//new SelectListItem(){ Text = "Sticker", Value = enumMessageType.Sticker.ToString() },
				new SelectListItem(){ Text = "Video", Value = enumMessageType.Video.ToString() },
	}
			};

			return View(sendMediaMessageViewModel);
        }

		/// <summary>
		/// This is now using SendMessageController	
		/// This will send Audio, Document, Image, Sticker, Video
		/// This is NOT to send Templates with the above media
		/// </summary>
		/// <param name="sendMediaMessage"></param>
		/// <returns></returns>
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppMediaMessage(SendMediaMessageViewModel sendMediaMessage)
        {
			try
			{
				SendWhatsAppPayload payload = new SendWhatsAppPayload();
				payload.SendText = new SendTextPayload()
				{
					ToNum = sendMediaMessage.RecipientPhoneNumber,
					Message = sendMediaMessage.Message
				};
				payload.MessageType = (enumMessageType)Enum.Parse(typeof(enumMessageType), sendMediaMessage.SelectedMediaType);

				//payload.MessageType = sendMediaMessage.SelectedMediaType;
				payload.Media = new WhatsAppMedia()
				{
					Type = "",
					URL = sendMediaMessage.MediaLink,
					ID = sendMediaMessage.MediaId,
					Caption = sendMediaMessage.Message
				};

				// Send the message and get the WAMId
				string WAMIds = _sendMessageController.GetWAMId((await _sendMessageController.SendWhatsApp_MediaAsync(payload)).Value);
                       

                if (WAMIds != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", $"Successfully sent media message with WAMId '{WAMIds}'");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppMediaMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppMediaMessage)).WithDanger("Error", ex.Message);
            }
        }

        public IActionResult SendWhatsAppLocationMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppLocationMessage(SendLocationMessageViewModel sendLocationMessageViewModel)
        {
            try
            {
				LocationMessageRequest locationMessageRequest = new LocationMessageRequest
				{
					To = sendLocationMessageViewModel.RecipientPhoneNumber,
					Location = new Location
					{
						Name = "Location Test",
						Address = "Address Test",
						Longitude = sendLocationMessageViewModel.Longitude,
						Latitude = sendLocationMessageViewModel.Latitude
					}
				};

				var results = await _whatsAppBusinessClient.SendLocationMessageAsync(locationMessageRequest);

                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent location message");
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }
        }

        public IActionResult SendWhatsAppInteractiveMessage()
        {
			SendInteractiveMessageViewModel sendInteractiveMessageViewModel = new SendInteractiveMessageViewModel
			{
				InteractiveType = new List<SelectListItem>()
			{
				new SelectListItem(){ Text = "List Message", Value = "List Message" },
				new SelectListItem(){ Text = "Reply Button", Value = "Reply Button" },
				new SelectListItem(){ Text = "Location Request Message", Value = "Location Request Message" }
			}
			};
			return View(sendInteractiveMessageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppInteractiveMessage(SendInteractiveMessageViewModel sendInteractiveMessageViewModel)
        {
            try
            {
                WhatsAppResponse results = null;

                if (sendInteractiveMessageViewModel.SelectedInteractiveType.Equals("List Message"))
                {
					InteractiveListMessageRequest interactiveListMessage = new InteractiveListMessageRequest
					{
						To = sendInteractiveMessageViewModel.RecipientPhoneNumber,
						Interactive = new InteractiveListMessage
						{
							Header = new Header
							{
								Type = "text",
								Text = "List Header Sample Test"
							},

							Body = new ListBody
							{
								Text = sendInteractiveMessageViewModel.Message
							},

							Footer = new Footer
							{
								Text = "List Footer Sample Test"
							},

							Action = new ListAction
							{
								Button = "Send",
								Sections = new List<Section>()
					{
						new Section()
						{
							Title = "Category A",
							Rows = new List<Row>()
							{
								new Row()
								{
									Id = "Item_A1",
									Title = "Apples",
									Description = "Enjoy fruits for free"
								},
								new Row()
								{
									Id = "Item_A2",
									Title = "Tangerines",
									Description = "Enjoy fruits for free"
								},
							},
						},
						new Section()
						{
							Title = "Category B",
							Rows = new List<Row>()
							{
								new Row()
								{
									Id = "Item_B1",
									Title = "2JZ",
									Description = "Engine discounts"
								},
								new Row()
								{
									Id = "Item_2",
									Title = "1JZ",
									Description = "Engine discounts"
								},
							}
						}
					}
							}
						}
					};

					results = await _whatsAppBusinessClient.SendInteractiveListMessageAsync(interactiveListMessage);
                }

                if (sendInteractiveMessageViewModel.SelectedInteractiveType.Equals("Reply Button"))
                {
					InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage = new InteractiveReplyButtonMessageRequest
					{
						To = sendInteractiveMessageViewModel.RecipientPhoneNumber,
						Interactive = new InteractiveReplyButtonMessage
						{
							Header = new ReplyButtonHeader
							{
								Type = "text",
								Text = "Reply Button Header Sample Test"
							},

							Body = new ReplyButtonBody
							{
								Text = sendInteractiveMessageViewModel.Message
							},

							Footer = new ReplyButtonFooter
							{
								Text = "Reply Button Footer Sample Test"
							},

							Action = new ReplyButtonAction
							{
								Buttons = new List<ReplyButton>()
					{
						new ReplyButton()
						{
							Type = "reply",
							Reply = new Reply()
							{
								Id = "SAMPLE_1_CLICK",
								Title = "CLICK ME!!!"
							}
						},

						new ReplyButton()
						{
							Type = "reply",
							Reply = new Reply()
							{
								Id = "SAMPLE_2_CLICK",
								Title = "LATER"
							}
						}
					}
							}
						}
					};

					results = await _whatsAppBusinessClient.SendInteractiveReplyButtonMessageAsync(interactiveReplyButtonMessage);
                }

                if (sendInteractiveMessageViewModel.SelectedInteractiveType.Equals("Location Request Message"))
                {
					InteractiveLocationMessageRequest interactiveLocationMessageRequest = new InteractiveLocationMessageRequest
					{
						To = sendInteractiveMessageViewModel.RecipientPhoneNumber,
						Interactive = new InteractiveLocationRequestMessage
						{
							Body = new InteractiveLocationBody
							{
								Text = (!string.IsNullOrWhiteSpace(sendInteractiveMessageViewModel.Message)) ? sendInteractiveMessageViewModel.Message : "Let us start with your pickup. You can either manually *enter an address* or *share your current location*."
							},
							Action = new InteractiveLocationAction()
						}
					};

					results = await _whatsAppBusinessClient.SendLocationRequestMessageAsync(interactiveLocationMessageRequest);
				}

                if (results != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent interactive message");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppInteractiveMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppInteractiveMessage)).WithDanger("Error", ex.Message);
            }
        }

        public IActionResult SendWhatsAppFlowMessage()
        {
			SendFlowMessageViewModel sendFlowMessageViewModel = new SendFlowMessageViewModel
			{
				FlowAction = new List<SelectListItem>()
			{
				new SelectListItem(){ Text = "Navigate", Value = "navigate" },
				new SelectListItem(){ Text = "Data Exchange", Value = "data_exchange" }
			},
				Mode = new List<SelectListItem>()
			{
				new SelectListItem(){ Text = "Draft", Value = "Draft" },
				new SelectListItem(){ Text = "Published", Value = "Published" }
			}
			};
			return View(sendFlowMessageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppFlowMessage(SendFlowMessageViewModel sendFlowMessageViewModel)
        {
            try
            {
				FlowMessageRequest flowMessageRequest = new FlowMessageRequest
				{
					To = sendFlowMessageViewModel.RecipientPhoneNumber,
					Interactive = new FlowMessageInteractive
					{
						Header = new FlowMessageHeader
						{
							Type = "text",
							Text = "Header flow"
						},

						Body = new FlowMessageBody
						{
							Text = "Body flow"
						},

						Footer = new FlowMessageFooter
						{
							Text = "Footer flow"
						},

						Action = new FlowMessageAction
						{
							Parameters = new FlowMessageParameters
							{
								FlowToken = sendFlowMessageViewModel.FlowToken,
								FlowId = sendFlowMessageViewModel.FlowId,
								FlowCta = sendFlowMessageViewModel.FlowButtonText,
								FlowAction = sendFlowMessageViewModel.SelectedFlowAction,
								IsInDraftMode = (sendFlowMessageViewModel.SelectedMode.Equals("Draft", StringComparison.OrdinalIgnoreCase)),

								FlowActionPayload = new FlowActionPayload
								{
									Screen = sendFlowMessageViewModel.ScreenId
								}
							}
						}
					}
				};

				var results = await _whatsAppBusinessClient.SendFlowMessageAsync(flowMessageRequest);

				return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent flow message");
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppFlowMessage)).WithDanger("Error", ex.Message);
			}
		}

        public IActionResult SendWhatsAppTemplateMessage()
        {
            return View();
        }

		/// <summary>
		/// This is to handle:
		/// 1. Plain Text messgaes
		/// 2. Text Templates (NO params)
		/// 3. Text Templates with Params
		/// </summary>
		/// <param name="payload"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendWhatsAppTextMessage(SendTemplateMessageViewModel payload)
		{ // Functional using SendMessageController
			try
			{
				SendWhatsAppPayload sendPayload = new();
				sendPayload.SendText = new SendTextPayload()
				{ ToNum = payload.RecipientPhoneNumber };
				sendPayload.SendText.PreviewUrl = false;

				if (payload.Message != null)
				{   // This is a normal plain Text Message
					sendPayload.SendText.Message = payload.Message;
				}
				else
				{   // This is a Template Test Message 
					sendPayload.Template = new WhatsappTemplate();
					sendPayload.Template.Name = payload.TemplateName;

					// CJM to add a Params Textbox on the Form					
					if (payload.TemplateParams != null)
					{
						string strParams = payload.TemplateParams; // "Cornelius#DAFP";
						List<string> listParams = strParams.Split(new string[] { "#" }, StringSplitOptions.None).ToList();
						sendPayload.Template.Params = listParams;
					}
				}

				// Send the message and get the WAMId
				string WAMIds = _sendMessageController.GetWAMId((await _sendMessageController.SendWhatsApp_TextAsync(sendPayload)).Value);


				if (WAMIds != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", $"Successfully sent video template message with WAMId '{WAMIds}'");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}

			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppInteractiveTemplateMessageWithParameters(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				// Tested with facebook predefined template name: sample_issue_resolution
				InteractiveTemplateMessageRequest interactiveTemplateMessage = new InteractiveTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new InteractiveMessageTemplate
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new InteractiveMessageLanguage
						{
							Code = LanguageCode.English_US
						},
						Components = new List<InteractiveMessageComponent>()
					}
				};
				interactiveTemplateMessage.Template.Components.Add(new InteractiveMessageComponent()
                {
                    Type = "body",
                    Parameters = new List<InteractiveMessageParameter>()
                    {
                        new InteractiveMessageParameter()
                        {
                            Type = "text",
                            Text = "Interactive Parameter Placeholder Position 1"
                        }
                    }
                });

                var results = await _whatsAppBusinessClient.SendInteractiveTemplateMessageAsync(interactiveTemplateMessage);

                if (results != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent interactive template message");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppMediaTemplateMessageWithParameters(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
			{
				SendWhatsAppPayload payload = new();
				payload.SendText = new SendTextPayload()
				{
					ToNum = sendTemplateMessageViewModel.RecipientPhoneNumber
				};
				payload.Template = new WhatsappTemplate();
				payload.Template.Name = sendTemplateMessageViewModel.TemplateName;

				// CJM to add a Params Textbox on the Form if it is empty then there are no params
				if (sendTemplateMessageViewModel.TemplateParams != null)
				{
					string strParams = sendTemplateMessageViewModel.TemplateParams; // "Cornelius#DAFP";
					List<string> listParams = strParams.Split(new string[] { "#" }, StringSplitOptions.None).ToList();

					payload.Template.Params = listParams;
				};

				payload.Media = new WhatsAppMedia
				{
					ID = !string.IsNullOrEmpty(sendTemplateMessageViewModel.MediaId) ? sendTemplateMessageViewModel.MediaId : null,
					URL = string.IsNullOrEmpty(sendTemplateMessageViewModel.MediaId) ? sendTemplateMessageViewModel.LinkUrl : null,
					Type = "image"      //,
										//	Caption = ""		// Caption does not work
				};

				// Send the message and get the WAMId
				string WAMIds = _sendMessageController.GetWAMId((await _sendMessageController.SendWhatsApp_TemplateImage_ParameterAsync(payload)).Value);


				if (WAMIds != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", $"Successfully sent video template message with WAMId '{WAMIds}'");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
				
				// Remember Other types of Parameters could be used, for now we will focus on Text only

				//			new ImageMessageComponent()
				//			{
				//				Type = "body",
				//				Parameters = new List<ImageMessageParameter>()
				//				{
				//					new ImageMessageParameter()
				//					{
				//						Type = "text",
				//						Text = "Movie Testing"
				//					},

				//					new ImageMessageParameter()
				//					{
				//						Type = "date_time",
				//						DateTime = new ImageTemplateDateTime()
				//						{
				//							FallbackValue = DateTime.Now.ToString("dddd d, yyyy"),
				//							DayOfWeek = (int)DateTime.Now.DayOfWeek,
				//							Year = DateTime.Now.Year,
				//							Month = DateTime.Now.Month,
				//							DayOfMonth = DateTime.Now.Day,
				//							Hour = DateTime.Now.Hour,
				//							Minute = DateTime.Now.Minute,
				//							Calendar = "GREGORIAN"
				//						}
				//					},

				//					new ImageMessageParameter()
				//					{
				//						Type = "text",
				//						Text = "Venue Test"
				//					},

				//					new ImageMessageParameter()
				//					{
				//						Type = "text",
				//						Text = "Seat 1A, 2A, 3A and 4A"
				//					}
				//				}
				//			}
				//		}
				//	}
				//};

            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppDocumentTemplateMessageWithParameters(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				DocumentTemplateMessageRequest documentTemplateMessage = new DocumentTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new DocumentMessageTemplate
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new DocumentMessageLanguage
						{
							Code = LanguageCode.English_US
						},
						Components = new List<DocumentMessageComponent>()
						{
							new DocumentMessageComponent()
							{
								Type = "header",
								Parameters = new List<DocumentMessageParameter>()
								{
									new DocumentMessageParameter()
									{
										Type = "document",
										Document = new Document()
										{
											//Id = payload.MediaId,
											Link = "<EXTERNAL_LINK_DOCUMENT>" // Link point where your document can be downloaded or retrieved by WhatsApp
										}
									}
								},
							},
							new DocumentMessageComponent()
							{
								Type = "body",
								Parameters = new List<DocumentMessageParameter>()
								{
									new DocumentMessageParameter()
									{
										Type = "text",
										Text = "Order Invoice"
									},
								}
							}
						}
					}
				};

				var results = await _whatsAppBusinessClient.SendDocumentAttachmentTemplateMessageAsync(documentTemplateMessage);

                if (results != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent document template message");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
            }
        }

		/// <summary>
		/// Making use of SendMessageController to send a WhatsApp Video Template with or without parameters
		/// </summary>
		/// <param name="sendTemplateMessageViewModel"></param>
		/// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppVideoTemplateMessageWithParameters(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				SendWhatsAppPayload payload = new();
				payload.SendText = new SendTextPayload()
				{
					ToNum = sendTemplateMessageViewModel.RecipientPhoneNumber
				};
				payload.Template = new WhatsappTemplate();
				payload.Template.Name = sendTemplateMessageViewModel.TemplateName;

				// CJM to add a Params Textbox on the Form if it is empty then there are no params
				if (sendTemplateMessageViewModel.TemplateParams != null)
				{
					string strParams = sendTemplateMessageViewModel.TemplateParams; // "Cornelius#DAFP";
					List<string> listParams = strParams.Split(new string[] { "#" }, StringSplitOptions.None).ToList();

					payload.Template.Params = listParams;
				};

				payload.Media = new WhatsAppMedia
				{
					ID = !string.IsNullOrEmpty(sendTemplateMessageViewModel.MediaId) ? sendTemplateMessageViewModel.MediaId : null,
					URL = string.IsNullOrEmpty(sendTemplateMessageViewModel.MediaId) ? sendTemplateMessageViewModel.LinkUrl : null,
					Type = "video"      //,
										//	Caption = ""		// Caption does not work
				};
				
				// Send the message and get the WAMId
				string WAMIds = _sendMessageController.GetWAMId((await _sendMessageController.SendWhatsApp_TemplateVideo_ParameterAsync(payload)).Value);


				if (WAMIds != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", $"Successfully sent video template message with WAMId '{WAMIds}'");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}

			catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
            }
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendWhatsAppAuthenticationTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest = new()
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<AuthenticationMessageComponent>()
				{
					new AuthenticationMessageComponent()
					{
						Type = "body",
						Parameters = new List<AuthenticationMessageParameter>()
						{
							new AuthenticationMessageParameter()
							{
								Type = "text",
								Text = "J$FpnYnP" // One time password value
							}
						}
					},
					new AuthenticationMessageComponent()
					{
						Type = "button",
						SubType = "url",
						Index = 0,
						Parameters = new List<AuthenticationMessageParameter>()
						{
							new AuthenticationMessageParameter()
							{
								Type = "text",
								Text = "J$FpnYnP" // One time password value
							}
						}
					}
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendAuthenticationMessageTemplateAsync(authenticationTemplateMessageRequest);

				if (results != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent authentication template message");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppCatalogueTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				CatalogTemplateMessageRequest catalogTemplateMessageRequest = new CatalogTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<CatalogMessageComponent>()
				{
					new CatalogMessageComponent()
					{
						Type = "Body",
						Parameters = new List<CatalogTemplateMessageParameter>()
						{
							new CatalogTemplateMessageParameter()
							{
								Type = "text",
								Text = "100"
							},
							new CatalogTemplateMessageParameter()
							{
								Type = "text",
								Text = "400"
							},
							new CatalogTemplateMessageParameter()
							{
								Type = "text",
								Text = "3"
							},
						}
					},
					new CatalogMessageComponent()
					{
						Type = "button",
						SubType = "CATALOG",
						Index = 0,
						Parameters = new List<CatalogTemplateMessageParameter>()
						{
							new CatalogTemplateMessageParameter()
							{
								Type = "action",
								Action = new CatalogTemplateMessageAction()
								{
									ThumbnailProductRetailerId = "2lc20305pt"
								}
							}
						}
					}
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendCatalogMessageTemplateAsync(catalogTemplateMessageRequest);

				if (results != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent catalogue template message");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendWhatsAppCarouselTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
		{
			try
			{
				CarouselTemplateMessageRequest carouselTemplateMessageRequest = new CarouselTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<CarouselMessageTemplateComponent>()
				{
					new CarouselMessageTemplateComponent()
					{
						Type = "BODY",
						Parameters = new List<CarouselMessageParameter>()
						{
							new CarouselMessageParameter()
							{
								Type = "Text",
								Text = "20OFF"
							},
							new CarouselMessageParameter()
							{
								Type = "Text",
								Text = "20%"
							}
						}
					},
					new CarouselMessageTemplateComponent()
					{
						Type = "CAROUSEL",
						Cards = new List<CarouselMessageCard>()
						{
							new CarouselMessageCard()
							{
								CardIndex = 0,
								Components = new List<CarouselCardComponent>()
								{
									new CarouselCardComponent()
									{
										Type = "HEADER",
										Parameters = new List<CardMessageParameter>()
										{
											new CardMessageParameter()
											{
												Type = "IMAGE",
												Image = new CardImage()
												{
													Id = "24230790383178626"
												}
											}
										}
									},
									new CarouselCardComponent()
									{
										Type = "BODY",
										Parameters = new List<CardMessageParameter>()
										{
											new CardMessageParameter()
											{
												Type = "Text",
												Text = "10OFF"
											},
											new CardMessageParameter()
											{
												Type = "Text",
												Text = "10%"
											}
										}
									},
									new CarouselCardComponent()
									{
										Type = "BUTTON",
										SubType = "QUICK_REPLY",
										Index = 0,
										Parameters = new List<CardMessageParameter>()
										{
											new CardMessageParameter()
											{
												Type = "PAYLOAD",
												Payload = "59NqSd"
											}
										}
									},
									new CarouselCardComponent()
									{
										Type = "button",
										SubType = "URL",
										Index = 1,
										Parameters = new List<CardMessageParameter>()
										{
											new CardMessageParameter()
											{
												Type = "PAYLOAD",
												Payload = "last_chance_2023"
											}
										}
									}
								}
							}
						}
					},
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendCarouselMessageTemplateAsync(carouselTemplateMessageRequest);

				if (results != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent carousel template message");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendWhatsAppCouponCodeTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
		{
			try
			{
				CouponCodeTemplateMessageRequest couponCodeTemplateMessageRequest = new CouponCodeTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<CouponCodeMessageComponent>()
				{
					new CouponCodeMessageComponent()
					{
						Type = "body",
						Parameters = new List<CouponCodeMessageParameter>()
						{
							new CouponCodeMessageParameter()
							{
								Type = "text",
								Text = "25OFF"
							},
							new CouponCodeMessageParameter()
							{
								Type = "text",
								Text = "25%"
							}
						}
					},
					new CouponCodeMessageComponent()
					{
						Type = "button",
						SubType = "COPY_CODE",
						Index = 1,
						Parameters = new List<CouponCodeMessageParameter>()
						{
							new CouponCodeMessageParameter()
							{
								Type = "coupon_code",
								Text = "25OFF"
							}
						}
					}
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendCouponCodeMessageTemplateAsync(couponCodeTemplateMessageRequest);

				if (results != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent coupon code template message");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppLimitedTimeOfferTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				LimitedTimeOfferTemplateMessageRequest limitedTimeOfferTemplateMessageRequest = new LimitedTimeOfferTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<LimitedTimeOfferMessageComponent>()
				{
					new LimitedTimeOfferMessageComponent()
					{
						Type = "body",
						Parameters = new List<LimitedTimeOfferMessageParameter>()
						{
							new LimitedTimeOfferMessageParameter()
							{
								Type = "text",
								Text = "Pablo"
							},
							new LimitedTimeOfferMessageParameter()
							{
								Type = "text",
								Text = "CARIBE25"
							}
						}
					},
					new LimitedTimeOfferMessageComponent()
					{
						Type = "limited_time_offer",
						Parameters = new List<LimitedTimeOfferMessageParameter>()
						{
							new LimitedTimeOfferMessageParameter()
							{
								Type = "limited_time_offer",
								LimitedTimeOffer = new LimitedTimeOffer()
								{
									ExpirationTimeMs = new DateTimeOffset(DateTime.UtcNow.AddHours(2)).ToUnixTimeMilliseconds()
								}
							}
						}
					},
					new LimitedTimeOfferMessageComponent()
					{
						Type = "button",
						SubType = "copy_code",
						Index = 0,
						Parameters = new List<LimitedTimeOfferMessageParameter>()
						{
							new LimitedTimeOfferMessageParameter()
							{
								Type = "coupon_code",
								CouponCode = "CARIBE25"
							}
						}
					},
					new LimitedTimeOfferMessageComponent()
					{
						Type = "button",
						SubType = "url",
						Index = 1,
						Parameters = new List<LimitedTimeOfferMessageParameter>()
						{
							new LimitedTimeOfferMessageParameter()
							{
								Type = "text",
								Text = "https://www.google.com/maps"
							}
						}
					}
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendLimitedTimeOfferMessageTemplateAsync(limitedTimeOfferTemplateMessageRequest);

				if (results != null)
				{
					return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent limited time offer template message");
				}
				else
				{
					return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppFlowTemplateMessage(SendTemplateMessageViewModel sendTemplateMessageViewModel)
        {
            try
            {
				FlowTemplateMessageRequest flowTemplateMessageRequest = new FlowTemplateMessageRequest
				{
					To = sendTemplateMessageViewModel.RecipientPhoneNumber,
					Template = new()
					{
						Name = sendTemplateMessageViewModel.TemplateName,
						Language = new()
						{
							Code = LanguageCode.English_US
						},
						Components = new List<FlowMessageComponent>()
				{
					new FlowMessageComponent()
					{
						Type = "button",
						SubType = "flow",
						Index = 0,
						Parameters = new List<FlowTemplateMessageParameter>()
						{
							new FlowTemplateMessageParameter()
							{
								Type = "action",
								Action = new FlowTemplateMessageAction()
								{
									FlowToken = "",
								}
							}
						}
					}
				}
					}
				};

				var results = await _whatsAppBusinessClient.SendFlowMessageTemplateAsync(flowTemplateMessageRequest);

                if (results != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent flow template message");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppTemplateMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(SendWhatsAppTemplateMessage)).WithDanger("Error", ex.Message);
            }
        }

		public IActionResult SendWhatsAppContactMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendWhatsAppContactMessage(SendContactMessageViewModel sendContactMessageViewModel)
        {
            try
            {
				ContactMessageRequest contactMessageRequest = new ContactMessageRequest
				{
					To = sendContactMessageViewModel.RecipientPhoneNumber,
					Contacts = new List<ContactData>()
				{
					new ContactData()
					{
						Addresses = new List<Address>()
						{
							new Address()
							{
								State = "State Test",
								City = "City Test",
								Zip = "Zip Test",
								Country = "Country Test",
								CountryCode = "Country Code Test",
								Type = "Home"
							}
						},
						Name = new Name()
						{
							FormattedName = "Testing name",
							FirstName = "FName",
							LastName = "LName",
							MiddleName = "MName"
						}
					}
				}
				};

				var results = await _whatsAppBusinessClient.SendContactAttachmentMessageAsync(contactMessageRequest);

                if (results != null)
                {
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Successfully sent contact message");
                }
                else
                {
                    return RedirectToAction(nameof(SendWhatsAppContactMessage));
                }
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }
        }

		public IActionResult BulkSendWhatsApps()
		{
			BulkSendWhatsAppsViewModel bulkSendWhatsAppsViewModel = new BulkSendWhatsAppsViewModel();
			
			return View(bulkSendWhatsAppsViewModel);
		}

		/// <summary>
		/// Make use of BulkSendWhatsAppController to read a CSV file, loop through the file and send whatsApp per record
		/// </summary>
		/// <param name="bulkSendWhatsAppsViewModel"></param>
		/// <param name="bulkFile"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> BulkSendWhatsApps(BulkSendWhatsAppsViewModel bulkSendWhatsAppsViewModel, IFormFile bulkFile)
		{
            try
            { // This is to call the relevant methods to run through the file and Bulk Send WhatsApps
			  //List<string> WAMIds = new List<string>();			

                // Upload the Bulk File to the Local Server
                FileInfo fileInfo = new();
                FileManagmentController fileController = new(_logger, _whatsAppBusinessClient, _environment);                               
                fileInfo = await fileController.UploadFileToLocalServer(bulkFile);

                // Now go through the file and send the WhatsApps
                BulkSendWhatsAppsController bulkSendWhatsAppsController = new(_logger, _whatsAppBusinessClient, _environment);
				var WAMIDs = bulkSendWhatsAppsController.ReadAndTraverseCSV(fileInfo);

				


				


                return View(bulkSendWhatsAppsViewModel);
            }
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return RedirectToAction(nameof(bulkFile)).WithDanger("Error", ex.Message);
			}
        }

		public IActionResult UploadMedia()
        {
			UploadMediaViewModel uploadMediaViewModel = new UploadMediaViewModel
			{
				UploadType = new List<SelectListItem>()
			{
				new SelectListItem(){ Text = "Normal Upload", Value = "Normal Upload" },
				new SelectListItem(){ Text = "Resumable Upload", Value = "Resumable Upload" },
			}
			};

			return View(uploadMediaViewModel);
        }
		
		/// <summary>
		/// This is to Upload files to WhatsApp
		/// NOTE: Resumable Uploads to WhatsApp does NOT provide a MediaID. To upload to WhatsApp ONLY use Normal Uploads
		/// Changed to make use of FileManagmentController
		/// </summary>
		/// <param name="uploadMediaViewModel"></param>
		/// <param name="mediaFile"></param>
		/// <returns></returns>
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMedia(UploadMediaViewModel uploadMediaViewModel, IFormFile mediaFile)
        {
            try
            {		                				
				FileInfo fileInfo = new();
				FileManagmentController fileToUpload = new(_logger, _whatsAppBusinessClient, _environment);				
                
                // Upload file to Local Server
                fileInfo = await fileToUpload.UploadFileToLocalServer(mediaFile);

				if (uploadMediaViewModel.SelectedUploadType.Equals("Normal Upload", StringComparison.OrdinalIgnoreCase))
                { // Do a Normal Upload
					fileInfo.fileUploadMethod = "Normal";

					fileInfo = await fileToUpload.UploadFileToWhatsApp(fileInfo);
                    ViewBag.MediaId = fileInfo.fileWhatsAppID;
				}
                else
                { // Do a Resumanble upload  ************* BUT ************** This is not presenting a Media ID so cannot be used after
                    fileInfo.fileUploadMethod = "Resumable";
				    //Upload file from Local Server to WhatsApp
				    fileInfo = await fileToUpload.UploadFileToWhatsApp(fileInfo);
                    ViewBag.H = fileInfo.fileResumableInfo.H;
                    ViewBag.StatusId = fileInfo.fileResumableInfo.StatusID;
				    ViewBag.FileOffset = fileInfo.fileResumableInfo.FileOffset;
                }
			    
				return View(uploadMediaViewModel).WithSuccess("Success", "Successfully upload media.");
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(UploadMedia)).WithDanger("Error", ex.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		public IActionResult CreateQRCodeMessage()
		{
			QRCodeMessageViewModel qrCodeMessageViewModel = new QRCodeMessageViewModel
			{
				ImageFormat = new List<SelectListItem>()
			{
				new SelectListItem(){ Text = "SVG", Value = "SVG" },
				new SelectListItem(){ Text = "PNG", Value = "PNG" },
			}
			};

			return View(qrCodeMessageViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateQRCodeMessage(QRCodeMessageViewModel qrCodeMessageViewModel)
		{
            try
            {
                var results = await _whatsAppBusinessClient.CreateQRCodeMessageAsync(qrCodeMessageViewModel.Message, qrCodeMessageViewModel.SelectedImageFormat);

                if (results is not null)
                {
                    ViewBag.QRCodeId = results.Code;
                    ViewBag.QRCodeMessage = results.PrefilledMessage;
                    ViewBag.QRCodeUrl = results.QrImageUrl;
					return View(qrCodeMessageViewModel).WithSuccess("Success", "Successfully created QR code Message.");
				}
                else
                {
					return View(qrCodeMessageViewModel).WithDanger("Error", "QR code message is null");
				}
            }
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return View(qrCodeMessageViewModel).WithDanger("Error", ex.Message);
			}
		}

        public async Task<IActionResult> QRCodeMessageList()
        {
            try
            {
                var results = await _whatsAppBusinessClient.GetQRCodeMessageListAsync();

                if (results is not null)
                {
                    if (results.Data.Any())
                    {
                        return View(results.Data).WithSuccess("Success", "Successfully retrieved QR code Message List");
                    }
                    else
                    {
                        return View(results.Data).WithDanger("Error", "QR code message list is empty");
                    }
                }
                else
                {
                    return View().WithDanger("Error", "QR code message list not availble");
                }
            }
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return View().WithDanger("Error", ex.Message);
			}
		}

        public async Task<IActionResult> Analytics()
        {
            try
            {
				DateTime currentDate = DateTime.UtcNow;
				var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
				var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
				var results = await _whatsAppBusinessClient.GetAnalyticMetricsAsync(_whatsAppConfig.WhatsAppBusinessAccountId, startOfMonth, endOfMonth, Granularity.AnalyticsGranularity.MONTH);

				if (results is not null)
				{
					return View(results).WithSuccess("Success", "Analytics retrieved successfully");
				}
				else
				{
					return View().WithDanger("Error", "Analytics not available");
				}
			}
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return View().WithDanger("Error", ex.Message);
			}
		}

        public async Task<IActionResult> ConversationAnalytics()
        {
            try
            {
                DateTime currentDate = DateTime.UtcNow;
                var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                var results = await _whatsAppBusinessClient.GetConversationAnalyticMetricsAsync(_whatsAppConfig.WhatsAppBusinessAccountId, startOfMonth, endOfMonth, Granularity.ConversationGranularity.MONTHLY);

                if (results is not null)
                {
                    return View(results).WithSuccess("Success", "Conversation Analytics retrieved successfully");
                }
                else
                {
                    return View().WithDanger("Error", "Conversation Analytics not available");
                }
            }
			catch (WhatsappBusinessCloudAPIException ex)
			{
				_logger.LogError(ex, ex.Message);
				return View().WithDanger("Error", ex.Message);
			}
		}
	}
}