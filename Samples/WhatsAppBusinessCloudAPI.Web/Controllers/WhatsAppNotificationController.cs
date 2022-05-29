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

        public WhatsAppNotificationController(ILogger<WhatsAppNotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("receive/TextMessage")]
        public IActionResult ReceiveWhatsAppTextMessage([FromBody] TextMessageReceived textMessageReceived)
        {
            if (textMessageReceived is null)
            {
                return BadRequest(new
                {
                    Message = "Message not received"
                });
            }

            // Handle logic to process WhatsApp Messages
            _logger.LogInformation(JsonConvert.SerializeObject(textMessageReceived, Formatting.Indented));

            return Ok(new
            {
                Message = "Message is received"
            });
        }
    }
}
