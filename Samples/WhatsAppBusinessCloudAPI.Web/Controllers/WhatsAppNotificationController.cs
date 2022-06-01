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
        private TextMessage textMessage;
        private ImageMessage imageMessage;

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

            
            var messageType = Convert.ToString(messageReceived["entry"][0]["changes"][0]["value"]["messages"][0]["type"]);

            if (messageType.Equals("text"))
            {
                var textMessageReceived = JsonConvert.DeserializeObject<TextMessageReceived>(Convert.ToString(messageReceived)) as TextMessageReceived;
                textMessage = textMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages).FirstOrDefault();
                _logger.LogInformation(JsonConvert.SerializeObject(textMessage, Formatting.Indented));

                return Ok(new
                {
                    Message = "Text Message received"
                });
            }

            if (messageType.Equals("image"))
            {
                var imageMessageReceived = JsonConvert.DeserializeObject<ImageMessageReceived>(Convert.ToString(messageReceived)) as ImageMessageReceived;
                imageMessage = imageMessageReceived.Entry.SelectMany(x => x.Changes).SelectMany(x => x.Value.Messages).FirstOrDefault();
                _logger.LogInformation(JsonConvert.SerializeObject(imageMessage, Formatting.Indented));

                return Ok(new
                {
                    Message = "Image Message received"
                });
            }
            return Ok();
        }
    }
}
