﻿using Microsoft.AspNetCore.Mvc;
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


		[Route("Home/TalkToMe")]
		public IActionResult TalkToMe()
		{
			// Webhook code
			return Ok();
		}

		[HttpGet("Home/TalkToMe")]
		public ActionResult<string> ConfigureWhatsAppMessageWebhook([FromQuery(Name = "hub.mode")] string hubMode,
																		 [FromQuery(Name = "hub.challenge")] int hubChallenge,
																		 [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
		{
			return Ok(hubChallenge);
		}

		[HttpPost("Home/TalkToMe")]
		public IActionResult ReceiveWhatsAppTextMessage([FromBody] dynamic messageReceived)
		{
			// Logic to handle different type of messages received
			return Ok();
		}

	}
}