using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
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
        private JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            WriteIndented = true
        };

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
        public async Task<IActionResult> ReceiveWhatsAppTextMessage([FromBody] JsonElement messageReceived)
        {
            try
            {
                if (messageReceived.ValueKind == JsonValueKind.Undefined)
                {
                    return BadRequest(new
                    {
                        Message = "Message not received"
                    });
                }

                // Message status updates will be triggered in different scenarios
                var changesResult = messageReceived.GetProperty("entry")[0].GetProperty("changes")[0].GetProperty("value");

                if (changesResult.TryGetProperty("statuses", out JsonElement statuses))
                {
                    var messageStatus = statuses[0].GetProperty("status").GetString();

                    if (messageStatus.Equals("sent"))
                    {
                        var messageStatusReceived = JsonSerializer.Deserialize<UserInitiatedMessageSentStatus>(messageReceived.GetRawText());
                        var messageStatusResults = new List<UserInitiatedStatus>(messageStatusReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Statuses));
                        _logger.LogInformation(JsonSerializer.Serialize(messageStatusResults, JsonSerializerOptions));

                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }

                    if (messageStatus.Equals("delivered"))
                    {
                        var messageStatusReceived = JsonSerializer.Deserialize<UserInitiatedMessageDeliveredStatus>(messageReceived.GetRawText());
                        var messageStatusResults = new List<UserInitiatedMessageDeliveryStatus>(messageStatusReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Statuses));
                        _logger.LogInformation(JsonSerializer.Serialize(messageStatusResults, JsonSerializerOptions));

                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }

                    if (messageStatus.Equals("read"))
                    {
                        var messageStatusReceived = JsonSerializer.Deserialize<MessageStatusUpdateNotification>(messageReceived.GetRawText());
                        var messageStatusResults = new List<MessageStatus>(messageStatusReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Statuses));
                        _logger.LogInformation(JsonSerializer.Serialize(messageStatusResults, JsonSerializerOptions));

                        return Ok(new
                        {
                            Message = $"Message Status Received: {messageStatus}"
                        });
                    }
                }
                else
                {
                    var messageType = changesResult.GetProperty("messages")[0].GetProperty("type").GetString();

					if (messageType.Equals("text"))
                    {
                        var textMessageReceived = JsonSerializer.Deserialize<MessageReceived<TextMessage>>(messageReceived.GetRawText());
                        textMessage = new List<TextMessage>(textMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
						_logger.LogInformation(JsonSerializer.Serialize(textMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(textMessageReceived));

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
                        var imageMessageReceived = JsonSerializer.Deserialize<MessageReceived<ImageMessage>>(messageReceived.GetRawText());
                        imageMessage = new List<ImageMessage>(imageMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(imageMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(imageMessageReceived));

                        return Ok(new
                        {
                            Message = "Image Message received"
                        });
                    }

                    if (messageType.Equals("audio"))
                    {
                        var audioMessageReceived = JsonSerializer.Deserialize<MessageReceived<AudioMessage>>(messageReceived.GetRawText());
                        audioMessage = new List<AudioMessage>(audioMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(audioMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(audioMessageReceived));

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
                        var documentMessageReceived = JsonSerializer.Deserialize<MessageReceived<DocumentMessage>>(messageReceived.GetRawText());
                        documentMessage = new List<DocumentMessage>(documentMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(documentMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(documentMessageReceived));

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
                        var stickerMessageReceived = JsonSerializer.Deserialize<MessageReceived<StickerMessage>>(messageReceived.GetRawText());
                        stickerMessage = new List<StickerMessage>(stickerMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(stickerMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(stickerMessageReceived));

                        return Ok(new
                        {
                            Message = "Sticker Message received"
                        });
                    }

                    if (messageType.Equals("contacts"))
                    {
                        var contactMessageReceived = JsonSerializer.Deserialize<MessageReceived<ContactMessage>>(messageReceived.GetRawText());
                        contactMessage = new List<ContactMessage>(contactMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(contactMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(contactMessageReceived));

                        return Ok(new
                        {
                            Message = "Contact Message Received"
                        });
                    }

                    if (messageType.Equals("location"))
                    {
                        var locationMessageReceived = JsonSerializer.Deserialize<MessageReceived<LocationMessage>>(messageReceived.GetRawText());
                        locationMessage = new List<LocationMessage>(locationMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(locationMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(locationMessageReceived));

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
                        var quickReplyMessageReceived = JsonSerializer.Deserialize<MessageReceived<QuickReplyButtonMessage>>(messageReceived.GetRawText());
                        quickReplyButtonMessage = new List<QuickReplyButtonMessage>(quickReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonSerializer.Serialize(quickReplyButtonMessage, JsonSerializerOptions));

                        await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(quickReplyMessageReceived));

                        return Ok(new
                        {
                            Message = "Quick Reply Button Message Received"
                        });
                    }

                    if (messageType.Equals("interactive"))
                    {
                        var getInteractiveType = changesResult.GetProperty("messages")[0].GetProperty("interactive").GetProperty("type").GetString();

                        if (getInteractiveType.Equals("button_reply"))
                        {
                            var replyMessageReceived = JsonSerializer.Deserialize<MessageReceived<ReplyButtonMessage>>(messageReceived.GetRawText());
                            replyButtonMessage = new List<ReplyButtonMessage>(replyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                            var buttonReply = replyButtonMessage.FirstOrDefault().Interactive.ButtonReply;
                            _logger.LogInformation(JsonSerializer.Serialize(replyButtonMessage, JsonSerializerOptions));

                            await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(replyMessageReceived));

                            return Ok(new
                            {
                                Message = "Reply Button Message Received"
                            });
                        }

                        if (getInteractiveType.Equals("list_reply"))
                        {
                            var listReplyMessageReceived = JsonSerializer.Deserialize<MessageReceived<ListReplyButtonMessage>>(messageReceived.GetRawText());
                            listReplyButtonMessage = new List<ListReplyButtonMessage>(listReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                            _logger.LogInformation(JsonSerializer.Serialize(listReplyButtonMessage, JsonSerializerOptions));

                            await _whatsAppBusinessClient.MarkMessageAsReadAsync(await GetMarkMessageRequestAsync(listReplyMessageReceived));

                            return Ok(new
                            {
                                Message = "List Reply Message Received"
                            });
                        }

                        if (getInteractiveType.Equals("nfm_reply")) // Flow message received
                        {
                            var flowMessageReceived = JsonSerializer.Deserialize<FlowMessageReceived>(messageReceived.GetRawText());
                            flowMessage = new List<FlowMessage>(flowMessageReceived.Messages);
                            _logger.LogInformation(JsonSerializer.Serialize(flowMessage, JsonSerializerOptions));
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

        [NonAction]
        public async Task<MarkMessageRequest> GetMarkMessageRequestAsync<TType>(MessageReceived<TType> message) where TType : IGenericMessage
        {
            var messages = new List<TType>(message.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
            MarkMessageRequest markMessageRequest = new MarkMessageRequest();
            markMessageRequest.MessageId = messages.SingleOrDefault().Id;
            markMessageRequest.Status = "read";
            markMessageRequest.TypingIndicator = new TypingIndicator();

			return markMessageRequest;

        }
    }
}
