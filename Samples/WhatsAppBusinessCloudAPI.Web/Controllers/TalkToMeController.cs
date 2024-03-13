using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using System.Globalization;
using CsvHelper;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsAppBusinessCloudAPI.Web.Extensions.Alerts;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.Webhook;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{		
	public class TalkToMeController : ControllerBase
	{
		private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
		private readonly ILogger<HomeController> _logger;
		private readonly IWebHostEnvironment _environment;
		private readonly List<MessageType> _msgTypes;
		private readonly string _webHookToken;
		public string MessagesReceived;

		public TalkToMeController ()
		{
			// Build configuration
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Access values
			_webHookToken = configuration["WhatsApp:WebHookToken"] ?? "Cannot be Null";
			MessagesReceived = "";
		}

		[HttpGet]
		[Route("Home/TalkToMe")]		
		public ActionResult<string> ConfigureWhatsAppMessageWebhook([FromQuery(Name = "hub.mode")] string hubMode,
																		 [FromQuery(Name = "hub.challenge")] int hubChallenge,
																		 [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
		{	
			if (hubVerifyToken != _webHookToken)
			{
				// Tokens not equal Stop processing
				return Ok(-1);
			}

			Console.WriteLine("______________________ Auth ____________________________");
			Console.WriteLine($" {hubMode}, {hubChallenge}, {hubVerifyToken}");

			return Ok(hubChallenge);
		}

		[HttpPost]		
		[Route("Home/TalkToMe")]
		public IActionResult ReceiveWhatsAppTextMessage([FromBody] dynamic messageReceived)
		{
			MessagesReceived += Environment.NewLine + "______________________ ReciveMessage ____________________________" + 
				Environment.NewLine + messageReceived;

			Console.WriteLine("______________________ ReciveMessage ____________________________");
			Console.WriteLine($"{messageReceived}");

			// Logic to handle different type of messages received
			return Ok();
		}

	}
}
