using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Exceptions;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Webhook;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppNotificationController : ControllerBase
    {
        private readonly ILogger<WhatsAppNotificationController> _logger;
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
        private readonly WhatsAppBusinessCloudApiConfig _whatsAppConfig;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string VerifyToken = "<YOUR VERIFY TOKEN STRING>";
        private List<TextMessage> textMessage;
        private List<AudioMessage> audioMessage;
        private List<ImageMessage> imageMessage;
        private List<DocumentMessage> documentMessage;
        private List<StickerMessage> stickerMessage;
        private List<ContactMessage> contactMessage;
        private List<LocationMessage> locationMessage;
        private List<QuickReplyButtonMessage> quickReplyButtonMessage;
        private List<ReplyButtonMessage> replyButtonMessage;
        private List<ListReplyButtonMessage> listReplyButtonMessage;
        private List<FlowMessage> flowMessage;

        public WhatsAppNotificationController(ILogger<WhatsAppNotificationController> logger, IWhatsAppBusinessClient whatsAppBusinessClient,
            IOptions<WhatsAppBusinessCloudApiConfig> whatsAppConfig, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _whatsAppBusinessClient = whatsAppBusinessClient;
            _whatsAppConfig = whatsAppConfig.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        // Required step for configuring webhook to WhatsApp Cloud API
        // Make sure the verifytoken matches with the hubverifytoken returned from whatsapp.
        [HttpGet("receive/TextMessage")]
        public ActionResult<string> ConfigureWhatsAppMessageWebhook([FromQuery(Name = "hub.mode")] string hubMode,
                                                                    [FromQuery(Name = "hub.challenge")] int hubChallenge,
                                                                    [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
        {
            _logger.LogInformation("Results Returned from WhatsApp Server\n");
            _logger.LogInformation($"hub_mode={hubMode}\n");
            _logger.LogInformation($"hub_challenge={hubChallenge}\n");
            _logger.LogInformation($"hub_verify_token={hubVerifyToken}\n");

            if (!hubVerifyToken.Equals(VerifyToken))
            {
                return Forbid("VerifyToken doesn't match");
            }
            return Ok(hubChallenge);
        }

        [HttpPost("receive/TextMessage")]
        public async Task<IActionResult> ReceiveWhatsAppTextMessage([FromBody] dynamic messageReceived)
        {
            try
            {
                if (messageReceived is null)
                {
                    return BadRequest(new
                    {
                        Message = "Message not received"
                    });
                }

                // Message status updates will be trigerred in different scenario
                var changesResult = messageReceived["entry"][0]["changes"][0]["value"];

                if (changesResult["statuses"] != null)
                {
                    var messageStatus = Convert.ToString(messageReceived["entry"][0]["changes"][0]["value"]["statuses"][0]["status"]);

                    if (messageStatus.Equals("sent"))
                    {
                        var messageStatusReceived = JsonConvert.DeserializeObject<UserInitiatedMessageSentStatus>(Convert.ToString(messageReceived)) as UserInitiatedMessageSentStatus;
                        var messageStatusResults = new List<UserInitiatedStatus>(messageStatusReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Statuses));
                        _logger.LogInformation(JsonConvert.SerializeObject(messageStatusResults, Formatting.Indented));

                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }

                    if (messageStatus.Equals("delivered"))
                    {
                        var messageStatusReceived = JsonConvert.DeserializeObject<UserInitiatedMessageDeliveredStatus>(Convert.ToString(messageReceived)) as UserInitiatedMessageDeliveredStatus;
                        var messageStatusResults = new List<UserInitiatedMessageDeliveryStatus>(messageStatusReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Statuses));
                        _logger.LogInformation(JsonConvert.SerializeObject(messageStatusResults, Formatting.Indented));

                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }

                    if (messageStatus.Equals("read"))
                    {
                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }
                }
                else
                {
                    var messageType = Convert.ToString(messageReceived["entry"][0]["changes"][0]["value"]["messages"][0]["type"]);

                    if (messageType.Equals("text"))
                    {
                        var textMessageReceived = JsonConvert.DeserializeObject<TextMessageReceived>(Convert.ToString(messageReceived)) as TextMessageReceived;
                        textMessage = new List<TextMessage>(textMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(textMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = textMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        TextMessageReplyRequest textMessageReplyRequest = new TextMessageReplyRequest();
                        textMessageReplyRequest.Context = new WhatsappBusiness.CloudApi.Messages.ReplyRequests.TextMessageContext();
                        textMessageReplyRequest.Context.MessageId = textMessage.SingleOrDefault().Id;
                        textMessageReplyRequest.To = textMessage.SingleOrDefault().From;
                        textMessageReplyRequest.Text = new WhatsAppText();
                        textMessageReplyRequest.Text.Body = "Your Message was received. Processing the request shortly";
                        textMessageReplyRequest.Text.PreviewUrl = false;

                        await _whatsAppBusinessClient.SendTextMessageAsync(textMessageReplyRequest);

                        return Ok(new
                        {
                            Message = "Text Message received"
                        });
                    }

                    if (messageType.Equals("image"))
                    {
                        var imageMessageReceived = JsonConvert.DeserializeObject<ImageMessageReceived>(Convert.ToString(messageReceived)) as ImageMessageReceived;
                        imageMessage = new List<ImageMessage>(imageMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(imageMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = imageMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        return Ok(new
                        {
                            Message = "Image Message received"
                        });
                    }

                    if (messageType.Equals("audio"))
                    {
                        var audioMessageReceived = JsonConvert.DeserializeObject<AudioMessageReceived>(Convert.ToString(messageReceived)) as AudioMessageReceived;
                        audioMessage = new List<AudioMessage>(audioMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(audioMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = audioMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        var mediaUrlResponse = await _whatsAppBusinessClient.GetMediaUrlAsync(audioMessage.SingleOrDefault().Audio.Id);

                        _logger.LogInformation(mediaUrlResponse.Url);

                        // To download media received sent by user
                        var mediaFileDownloaded = await _whatsAppBusinessClient.DownloadMediaAsync(mediaUrlResponse.Url);

                        var rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "Application_Files\\MediaDownloads\\");

                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }

                        // Get the path of filename
                        string filename = string.Empty;

                        if (mediaUrlResponse.MimeType.Contains("audio/mpeg"))
                        {
                            filename = $"{mediaUrlResponse.Id}.mp3";
                        }

                        if (mediaUrlResponse.MimeType.Contains("audio/ogg"))
                        {
                            filename = $"{mediaUrlResponse.Id}.ogg";
                        }

                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Application_Files\\MediaDownloads\\", filename);

                        await System.IO.File.WriteAllBytesAsync(filePath, mediaFileDownloaded);

                        return Ok(new
                        {
                            Message = "Audio Message received"
                        });
                    }

                    if (messageType.Equals("document"))
                    {
                        var documentMessageReceived = JsonConvert.DeserializeObject<DocumentMessageReceived>(Convert.ToString(messageReceived)) as DocumentMessageReceived;
                        documentMessage = new List<DocumentMessage>(documentMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(documentMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = documentMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        var mediaUrlResponse = await _whatsAppBusinessClient.GetMediaUrlAsync(documentMessage.SingleOrDefault().Document.Id);

                        _logger.LogInformation(mediaUrlResponse.Url);

                        // To download media received sent by user
                        var mediaFileDownloaded = await _whatsAppBusinessClient.DownloadMediaAsync(mediaUrlResponse.Url);

                        var rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "Application_Files\\MediaDownloads\\");

                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }

                        // Get the path of filename
                        string filename = string.Empty;

                        if (mediaUrlResponse.MimeType.Contains("audio/mpeg"))
                        {
                            filename = $"{mediaUrlResponse.Id}.mp3";
                        }

                        if (mediaUrlResponse.MimeType.Contains("audio/ogg"))
                        {
                            filename = $"{mediaUrlResponse.Id}.ogg";
                        }

                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Application_Files\\MediaDownloads\\", filename);

                        await System.IO.File.WriteAllBytesAsync(filePath, mediaFileDownloaded);

                        return Ok(new
                        {
                            Message = "Document Message received"
                        });
                    }

                    if (messageType.Equals("sticker"))
                    {
                        var stickerMessageReceived = JsonConvert.DeserializeObject<StickerMessageReceived>(Convert.ToString(messageReceived)) as StickerMessageReceived;
                        stickerMessage = new List<StickerMessage>(stickerMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(imageMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = stickerMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        return Ok(new
                        {
                            Message = "Image Message received"
                        });
                    }

                    if (messageType.Equals("contacts"))
                    {
                        var contactMessageReceived = JsonConvert.DeserializeObject<ContactMessageReceived>(Convert.ToString(messageReceived)) as ContactMessageReceived;
                        contactMessage = new List<ContactMessage>(contactMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(contactMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = contactMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        return Ok(new
                        {
                            Message = "Contact Message Received"
                        });
                    }


                    if (messageType.Equals("location"))
                    {
                        var locationMessageReceived = JsonConvert.DeserializeObject<StaticLocationMessageReceived>(Convert.ToString(messageReceived)) as StaticLocationMessageReceived;
                        locationMessage = new List<LocationMessage>(locationMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(locationMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = locationMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        LocationMessageReplyRequest locationMessageReplyRequest = new LocationMessageReplyRequest();
                        locationMessageReplyRequest.Context = new WhatsappBusiness.CloudApi.Messages.ReplyRequests.LocationMessageContext();
                        locationMessageReplyRequest.Context.MessageId = locationMessage.SingleOrDefault().Id;
                        locationMessageReplyRequest.To = locationMessage.SingleOrDefault().From;
                        locationMessageReplyRequest.Location = new WhatsappBusiness.CloudApi.Messages.Requests.Location();
                        locationMessageReplyRequest.Location.Name = "Location Test";
                        locationMessageReplyRequest.Location.Address = "Address Test";
                        locationMessageReplyRequest.Location.Longitude = -122.425332;
                        locationMessageReplyRequest.Location.Latitude = 37.758056;

                        await _whatsAppBusinessClient.SendLocationMessageAsync(locationMessageReplyRequest);

                        return Ok(new
                        {
                            Message = "Location Message Received"
                        });
                    }

                    if (messageType.Equals("button"))
                    {
                        var quickReplyMessageReceived = JsonConvert.DeserializeObject<QuickReplyButtonMessageReceived>(Convert.ToString(messageReceived)) as QuickReplyButtonMessageReceived;
                        quickReplyButtonMessage = new List<QuickReplyButtonMessage>(quickReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(quickReplyButtonMessage, Formatting.Indented));

                        MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                        markMessageRequest.MessageId = quickReplyButtonMessage.SingleOrDefault().Id;
                        markMessageRequest.Status = "read";

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                        return Ok(new
                        {
                            Message = "Quick Reply Button Message Received"
                        });
                    }

                    if (messageType.Equals("interactive"))
                    {
                        var getInteractiveType = Convert.ToString(messageReceived["entry"][0]["changes"][0]["value"]["messages"][0]["interactive"]["type"]);

                        if (getInteractiveType.Equals("button_reply"))
                        {
                            var replyMessageReceived = JsonConvert.DeserializeObject<ReplyButtonMessageReceived>(Convert.ToString(messageReceived)) as ReplyButtonMessageReceived;
                            replyButtonMessage = new List<ReplyButtonMessage>(replyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                            _logger.LogInformation(JsonConvert.SerializeObject(replyButtonMessage, Formatting.Indented));

                            MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                            markMessageRequest.MessageId = replyButtonMessage.SingleOrDefault().Id;
                            markMessageRequest.Status = "read";

                            await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                            return Ok(new
                            {
                                Message = "Reply Button Message Received"
                            });
                        }

                        if (getInteractiveType.Equals("list_reply"))
                        {
                            var listReplyMessageReceived = JsonConvert.DeserializeObject<ListReplyButtonMessageReceived>(Convert.ToString(messageReceived)) as ListReplyButtonMessageReceived;
                            listReplyButtonMessage = new List<ListReplyButtonMessage>(listReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                            _logger.LogInformation(JsonConvert.SerializeObject(listReplyButtonMessage, Formatting.Indented));

                            MarkMessageRequest markMessageRequest = new MarkMessageRequest();
                            markMessageRequest.MessageId = listReplyButtonMessage.SingleOrDefault().Id;
                            markMessageRequest.Status = "read";

                            await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

                            return Ok(new
                            {
                                Message = "List Reply Message Received"
                            });
                        }

                        if (getInteractiveType.Equals("nfm_reply")) // Flow message rceived
                        {
                            var flowMessageReceived = JsonConvert.DeserializeObject<FlowMessageReceived>(Convert.ToString(messageReceived)) as FlowMessageReceived;
                            flowMessage = new List<FlowMessage>(flowMessageReceived.Messages);
                            _logger.LogInformation(JsonConvert.SerializeObject(flowMessage, Formatting.Indented));
                            _logger.LogInformation($"User flow message sent: {flowMessage.SingleOrDefault().Interactive.NfmReply.Body}\n{flowMessage.SingleOrDefault().Interactive.NfmReply.ResponseJson}");

							MarkMessageRequest markMessageRequest = new MarkMessageRequest();
							markMessageRequest.MessageId = flowMessage.SingleOrDefault().Id;
							markMessageRequest.Status = "read";

							await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessageRequest);

							return Ok(new
                            {
                                Message = "Flow Message Received"
                            });
                        }
                    }
                }
                return Ok();
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
