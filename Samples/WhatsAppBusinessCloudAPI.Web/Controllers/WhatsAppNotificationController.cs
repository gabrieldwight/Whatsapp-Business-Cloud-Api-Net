using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WhatsappBusiness.CloudApi.Webhook;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppNotificationController : ControllerBase
    {
        private readonly ILogger<WhatsAppNotificationController> _logger;
        private string VerifyToken = "<YOUR VERIFY TOKEN STRING>";
        private List<TextMessage> textMessage;
        private List<ImageMessage> imageMessage;
        private List<StickerMessage> stickerMessage;
        private List<ContactMessage> contactMessage;
        private List<LocationMessage> locationMessage;
        private List<QuickReplyButtonMessage> quickReplyButtonMessage;
        private List<ListReplyButtonMessage> listReplyButtonMessage;

        public WhatsAppNotificationController(ILogger<WhatsAppNotificationController> logger)
        {
            _logger = logger;
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
        public IActionResult ReceiveWhatsAppTextMessage([FromBody] dynamic messageReceived)
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

                    return Ok(new
                    {
                        Message = "Image Message received"
                    });
                }

                if (messageType.Equals("sticker"))
                {
                    var stickerMessageReceived = JsonConvert.DeserializeObject<StickerMessageReceived>(Convert.ToString(messageReceived)) as StickerMessageReceived;
                    stickerMessage = new List<StickerMessage>(stickerMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                    _logger.LogInformation(JsonConvert.SerializeObject(imageMessage, Formatting.Indented));

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

                    return Ok(new
                    {
                        Message = "Location Message Received"
                    });
                }

                if (messageType.Equals("interactive"))
                {
                    var getInteractiveType = Convert.ToString(messageReceived["entry"][0]["changes"][0]["value"]["messages"][0]["interactive"]["type"]);

                    if (getInteractiveType.Equals("button_reply"))
                    {
                        var quickReplyMessageReceived = JsonConvert.DeserializeObject<QuickReplyButtonMessageReceived>(Convert.ToString(messageReceived)) as QuickReplyButtonMessageReceived;
                        quickReplyButtonMessage = new List<QuickReplyButtonMessage>(quickReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(quickReplyMessageReceived, Formatting.Indented));

                        return Ok(new
                        {
                            Message = "Quick Reply Button Message Received"
                        });
                    }

                    if (getInteractiveType.Equals("list_reply"))
                    {
                        var listReplyMessageReceived = JsonConvert.DeserializeObject<ListReplyButtonMessageReceived>(Convert.ToString(messageReceived)) as ListReplyButtonMessageReceived;
                        listReplyButtonMessage = new List<ListReplyButtonMessage>(listReplyMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages));
                        _logger.LogInformation(JsonConvert.SerializeObject(listReplyMessageReceived, Formatting.Indented));

                        return Ok(new
                        {
                            Message = "List Reply Message Received"
                        });
                    }
                }
            }
            return Ok();
        }
    }
}
