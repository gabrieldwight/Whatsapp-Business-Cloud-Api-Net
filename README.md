# WhatsApp Business Cloud API C# Wrapper Library for .NET Developers

A Wrapper for Whatsapp Business Cloud API hosted by Meta.

[![Build status](https://dev.azure.com/gabrieldwight/WhatsappCloudApi/_apis/build/status/WhatsappCloudApi-CI)](https://dev.azure.com/gabrieldwight/WhatsappCloudApi/_build/latest?definitionId=9)
[![NuGet version (WhatsappBusiness.CloudApi)](https://img.shields.io/nuget/v/WhatsappBusiness.CloudApi.svg?style=flat-square)](https://www.nuget.org/packages/WhatsappBusiness.CloudApi/)

Official API Documentation: [Meta for Developers](https://developers.facebook.com/docs/whatsapp/cloud-api/overview)

Sell Product and Services: [Product Message Configuration](https://developers.facebook.com/docs/whatsapp/cloud-api/guides/sell-products-and-services)

Account Metrics: [Account Metrics Configuration for analytics and conversation analytics](https://developers.facebook.com/docs/whatsapp/business-management-api/analytics)

QR Code Message Management: [QR Code Messages for WhatsApp Business](https://developers.facebook.com/docs/whatsapp/business-management-api/qr-codes)

WhatsApp Flows: [Setting up WhatsApp Flows](https://developers.facebook.com/docs/whatsapp/flows/gettingstarted/)

Webhook Configuration Documentation: [WhatsApp Cloud API Webhook](https://developers.facebook.com/docs/graph-api/webhooks/getting-started#verification-requests)

Authentication Message Documentation: [Create and Send Authentication Message](https://developers.facebook.com/docs/whatsapp/business-management-api/authentication-templates)

WhatsApp Cloud API Error Codes: [Error Codes](https://developers.facebook.com/docs/whatsapp/cloud-api/support/error-codes)

- [End User License](https://github.com/gabrieldwight/Whatsapp-Business-Cloud-Api-Net/blob/master/LICENSE)
- [NuGet Package](https://www.nuget.org/packages/WhatsappBusiness.CloudApi/)

Take note: Sending a message to a phone number format `00[Country Code] xx xx xx` using the prefix `00` before the country code will make the cloud API return an invalid parameter error (#100) (Credits @Tekkharibo)

Take note: Downloading media from the generated Whatsapp media URL will require one to specify the app name and version value to be set as useragent for the download media function to work. It is included as properties for the config class. (Credits @atmtrujillo)

## Capabilities

> Note: **This package is WIP**. The capabilities of Cloud API will be reflected soon. Feel free to contribute!

- [x] Sending Messages
  - [x] Text
  - [x] Media (image, video, audio, document, sticker)
  - [x] Contact
  - [x] Location
  - [x] Interactive (List, Reply)
  - [x] Template (text, image, video, document, authentication, flow message, carousel, catalog message, limited-time offer message, product message)
  - [x] Template Messages with parameters (text, image, video, document, authentication, product message)
  - [x] Single Product Message
  - [x] Multiple Product Message
  - [x] Authentication Message
  - [x] Flow Message
- [x] Receiving Message (via Webhook)
  - [x] Text
  - [x] Media (image, video, audio, document, sticker)
  - [x] Contact
  - [x] Location
  - [x] Interactive (List, Reply)
  - [x] Button

- [x] WhatsApp Business Management API
  - [x] QR Code Message Management
  - [x] Account Metrics

- [x] Sample project
  - [x] Read csv, and send over WhatsApp per record
  - [x] Implements many of the samples of below
  - [x] Upload files to local server, before uploading to WhatsApp
     

## Installation
- PackageManager: ```PM> Install-Package WhatsappBusiness.CloudApi```
- DotNetCLI: ```> dotnet add package WhatsappBusiness.CloudApi```

## Setting yourself for successful WhatsApp Business Cloud Api integration
Before you proceed kindly acquaint yourself with WhatsApp Business Cloud Apis by going through the Docs in Meta's developer portal if you like.

1.  Obtain a Temporary access token for the meta developers portal.

2.  Ensure your project is running on the minimum supported versions of .Net 

3.  WhatsAppBusinessCloudAPi is dependency injection (DI) friendly and can be readily injected into your classes. You can read more on DI in Asp.Net core [**here**](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0). If you can't use DI you can always manually create a new instance of WhatsAppBusinessClient and pass in an httpClient instance in it's constructor. eg.

```c#
// When Dependency Injection is not possible...

//create httpclient instance
var httpClient = new HttpClient();

httpClient.BaseAddress = WhatsAppBusinessRequestEndpoint.BaseAddress;
	
//create WhatsAppBusiness API client instance
var whatsAppBusinessClient = new WhatsAppBusinessClient(httpClient, whatsAppConfig); //make sure to pass httpclient and whatsAppConfig instance as an argument
	
```
I would recommend creating WhatsAppBusinessClient using Dependency Injection. [Optional] You can use any IOC container or Microsoft DI container in your legacy projects.
```c#
// Adding Dependency Injection into legacy projects

public static IServiceProvider ServiceProvider;


// To be used in the main application startup method
void Application_Start(object sender, EventArgs e)
{
  var hostBuilder = new HostBuilder();
  hostBuilder.ConfigureServices(ConfigureServices);
  var host = hostBuilder.Build();

  ServiceProvider = host.Services;
}

void ConfigureServices(IServiceCollection services)
{
   services.AddHttpClient<IWhatsAppBusinessClient, WhatsAppBusinessClient>(options => options.BaseAddress = WhatsAppBusinessRequestEndpoint.BaseAddress);
   //inject services here
}
	
```

## Registering WhatsAppBusinessClient & Set the BaseAddress -Dependency Injection Method in ASPNETCORE
* Install WhatsappBusiness.CloudApi Project via Nuget Package Manager Console or Nuget Package Manager GUI.

## For ASPNETCORE projects
* In **Program.cs** add the namespace...

```c#    
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;
```

* Inside ConfigureServices method add the following

```c#
WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
whatsAppConfig.WhatsAppBusinessPhoneNumberId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessPhoneNumberId"];
whatsAppConfig.WhatsAppBusinessAccountId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessAccountId"];
whatsAppConfig.WhatsAppBusinessId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessId"];
whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);
```

* Once the WhatsAppBusinessClient is registered, you can pass it and use it in your classes to make API calls to WhatsApp Business Cloud API Server as follows;
```c#
public class SendMessageController
{
	private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
	public SendMessageController(IWhatsAppBusinessClient whatsAppBusinessClient)
	{
		_whatsAppBusinessClient = whatsAppBusinessClient;
	}
	....
	//code omitted for brevity
}
```

## Send Text Message Request
```c#
TextMessageRequest textMessageRequest = new TextMessageRequest();
textMessageRequest.To = "Recipient Phone Number";
textMessageRequest.Text = new WhatsAppText();
textMessageRequest.Text.Body = "Message Body";
textMessageRequest.Text.PreviewUrl = false;

var results = await _whatsAppBusinessClient.SendTextMessageAsync(textMessageRequest);
```
## Send Audio Message Request
```c#
AudioMessageByUrlRequest audioMessage = new AudioMessageByUrlRequest();
audioMessage.To = "Recipient Phone Number";
audioMessage.Audio = new MediaAudioUrl();
audioMessage.Audio.Link = "Audio Url";

var results = await _whatsAppBusinessClient.SendAudioAttachmentMessageByUrlAsync(audioMessage);
```
## Send Document Message Request
```c#
DocumentMessageByUrlRequest documentMessage = new DocumentMessageByUrlRequest();
documentMessage.To = "Recipient Phone Number";
documentMessage.Document = new MediaDocumentUrl();
documentMessage.Document.Link = "Document Url";

var results = await _whatsAppBusinessClient.SendDocumentAttachmentMessageByUrlAsync(documentMessage);
```

## Send Image Message Request
```c#
ImageMessageByUrlRequest imageMessage = new ImageMessageByUrlRequest();
imageMessage.To = "Recipient Phone Number";
imageMessage.Image = new MediaImageUrl();
imageMessage.Image.Link = "Image Url";

var results = await _whatsAppBusinessClient.SendImageAttachmentMessageByUrlAsync(imageMessage);
```
## Send Sticker Message Request
```c#
StickerMessageByUrlRequest stickerMessage = new StickerMessageByUrlRequest();
stickerMessage.To = "Recipient Phone Number";
stickerMessage.Sticker = new MediaStickerUrl();
stickerMessage.Sticker.Link = "Sticker Url";

var results = await _whatsAppBusinessClient.SendStickerMessageByUrlAsync(stickerMessage);
```

## Send Video Message Request
```c#
VideoMessageByUrlRequest videoMessage = new VideoMessageByUrlRequest();
videoMessage.To = "Recipient Phone Number";
videoMessage.Video = new MediaVideoUrl();
videoMessage.Video.Link = "Video url";

var results = await _whatsAppBusinessClient.SendVideoAttachmentMessageByUrlAsync(videoMessage);
```

## Send Contact Message Request
```c#
ContactMessageRequest contactMessageRequest = new ContactMessageRequest();
contactMessageRequest.To = "Recipient Phone Number";
contactMessageRequest.Contacts = new List<ContactData>()
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
};

var results = await _whatsAppBusinessClient.SendContactAttachmentMessageAsync(contactMessageRequest);
```

## Send Location Message Request
```c#
LocationMessageRequest locationMessageRequest = new LocationMessageRequest();
locationMessageRequest.To = "Recipient Phone Number";
locationMessageRequest.Location = new Location();
locationMessageRequest.Location.Name = "Location Test";
locationMessageRequest.Location.Address = "Address Test";
locationMessageRequest.Location.Longitude = "location longitude";
locationMessageRequest.Location.Latitude = "location latitude";

var results = await _whatsAppBusinessClient.SendLocationMessageAsync(locationMessageRequest);
```

## Send Interactive List Message Request
```c#
InteractiveListMessageRequest interactiveListMessage = new InteractiveListMessageRequest();
interactiveListMessage.To = "Recipient Phone Number";
interactiveListMessage.Interactive = new InteractiveListMessage();

interactiveListMessage.Interactive.Header = new Header();
interactiveListMessage.Interactive.Header.Type = "text";
interactiveListMessage.Interactive.Header.Text = "List Header Sample Test";

interactiveListMessage.Interactive.Body = new ListBody();
interactiveListMessage.Interactive.Body.Text = "List Message Body";

interactiveListMessage.Interactive.Footer = new Footer();
interactiveListMessage.Interactive.Footer.Text = "List Footer Sample Test";

interactiveListMessage.Interactive.Action = new ListAction();
interactiveListMessage.Interactive.Action.Button = "Send";
interactiveListMessage.Interactive.Action.Sections = new List<Section>()
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
};

var results = await _whatsAppBusinessClient.SendInteractiveListMessageAsync(interactiveListMessage);
```
## Send Interactive Reply Button Request
```c#
InteractiveReplyButtonMessageRequest interactiveReplyButtonMessage = new InteractiveReplyButtonMessageRequest();
interactiveReplyButtonMessage.To = "Recipient Phone Number";
interactiveReplyButtonMessage.Interactive = new InteractiveReplyButtonMessage();

interactiveReplyButtonMessage.Interactive.Body = new ReplyButtonBody();
interactiveReplyButtonMessage.Interactive.Body.Text = "Reply Button Body";

interactiveReplyButtonMessage.Interactive.Action = new ReplyButtonAction();
interactiveReplyButtonMessage.Interactive.Action.Buttons = new List<ReplyButton>()
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
};

var results = await _whatsAppBusinessClient.SendInteractiveReplyButtonMessageAsync(interactiveReplyButtonMessage);
```

## Send Template Message Request
```c#
TextTemplateMessageRequest textTemplateMessage = new TextTemplateMessageRequest();
textTemplateMessage.To = "Recipient Phone Number";
textTemplateMessage.Template = new TextMessageTemplate();
textTemplateMessage.Template.Name = "Template Name";
textTemplateMessage.Template.Language = new TextMessageLanguage();
textTemplateMessage.Template.Language.Code = "en_US";

var results = await _whatsAppBusinessClient.SendTextMessageTemplateAsync(textTemplateMessage);
```

## Send Text Template Message with parameters request
```c#
// For Text Template message with parameters supported component type is body only
TextTemplateMessageRequest textTemplateMessage = new TextTemplateMessageRequest();
textTemplateMessage.To = sendTemplateMessageViewModel.RecipientPhoneNumber;
textTemplateMessage.Template = new TextMessageTemplate();
textTemplateMessage.Template.Name = sendTemplateMessageViewModel.TemplateName;
textTemplateMessage.Template.Language = new TextMessageLanguage();
textTemplateMessage.Template.Language.Code = LanguageCode.English_US;
textTemplateMessage.Template.Components = new List<TextMessageComponent>();
textTemplateMessage.Template.Components.Add(new TextMessageComponent()
{
    Type = "body",
    Parameters = new List<TextMessageParameter>()
    {
	new TextMessageParameter()
	{
	    Type = "text",
	    Text = "Testing Parameter Placeholder Position 1"
	},
	new TextMessageParameter()
	{
	    Type = "text",
	    Text = "Testing Parameter Placeholder Position 2"
	}
    }
});

var results = await _whatsAppBusinessClient.SendTextMessageTemplateAsync(textTemplateMessage);
```

## Send Media Template Message with parameters
```c#
// Tested with facebook predefined template name: sample_movie_ticket_confirmation
ImageTemplateMessageRequest imageTemplateMessage = new ImageTemplateMessageRequest();
imageTemplateMessage.To = sendTemplateMessageViewModel.RecipientPhoneNumber;
imageTemplateMessage.Template = new ImageMessageTemplate();
imageTemplateMessage.Template.Name = sendTemplateMessageViewModel.TemplateName;
imageTemplateMessage.Template.Language = new ImageMessageLanguage();
imageTemplateMessage.Template.Language.Code = LanguageCode.English_US;
imageTemplateMessage.Template.Components = new List<ImageMessageComponent>()
{
    new ImageMessageComponent()
    {
	Type = "header",
	Parameters = new List<ImageMessageParameter>()
	{
	    new ImageMessageParameter()
	    {
		Type = "image",
		Image = new Image()
		{
		    Link = "https://otakukart.com/wp-content/uploads/2022/03/Upcoming-Marvel-Movies-In-2022-23.jpg"
		}
	    }
	},
    },
    new ImageMessageComponent()
    {
	Type = "body",
	Parameters = new List<ImageMessageParameter>()
	{
	    new ImageMessageParameter()
	    {
		Type = "text",
		Text = "Movie Testing"
	    },

	    new ImageMessageParameter()
	    {
		Type = "date_time",
		DateTime = new ImageTemplateDateTime()
		{
		    FallbackValue = DateTime.Now.ToString("dddd d, yyyy"),
		    DayOfWeek = (int)DateTime.Now.DayOfWeek,
		    Year = DateTime.Now.Year,
		    Month = DateTime.Now.Month,
		    DayOfMonth = DateTime.Now.Day,
		    Hour = DateTime.Now.Hour,
		    Minute = DateTime.Now.Minute,
		    Calendar = "GREGORIAN"
		}
	    },

	    new ImageMessageParameter()
	    {
		Type = "text",
		Text = "Venue Test"
	    },

	    new ImageMessageParameter()
	    {
		Type = "text",
		Text = "Seat 1A, 2A, 3A and 4A"
	    }
	}
    }
};

var results = await _whatsAppBusinessClient.SendImageAttachmentTemplateMessageAsync(imageTemplateMessage);
```

## Send Interactive Template Message with parameters
```c#
// Tested with facebook predefined template name: sample_issue_resolution
InteractiveTemplateMessageRequest interactiveTemplateMessage = new InteractiveTemplateMessageRequest();
interactiveTemplateMessage.To = sendTemplateMessageViewModel.RecipientPhoneNumber;
interactiveTemplateMessage.Template = new InteractiveMessageTemplate();
interactiveTemplateMessage.Template.Name = sendTemplateMessageViewModel.TemplateName;
interactiveTemplateMessage.Template.Language = new InteractiveMessageLanguage();
interactiveTemplateMessage.Template.Language.Code = LanguageCode.English_US;
interactiveTemplateMessage.Template.Components = new List<InteractiveMessageComponent>();
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
```

## Sending Authentication Message
```c#
// You need to create your authentication template message
AuthenticationTemplateMessageRequest authenticationTemplateMessageRequest = new();
authenticationTemplateMessageRequest.To = sendTemplateMessageViewModel.RecipientPhoneNumber;
authenticationTemplateMessageRequest.Template = new();
authenticationTemplateMessageRequest.Template.Name = sendTemplateMessageViewModel.TemplateName;
authenticationTemplateMessageRequest.Template.Language = new();
authenticationTemplateMessageRequest.Template.Language.Code = LanguageCode.English_US;
authenticationTemplateMessageRequest.Template.Components = new List<AuthenticationMessageComponent>()
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
};

var results = await _whatsAppBusinessClient.SendAuthenticationMessageTemplateAsync(authenticationTemplateMessageRequest);
```

## Sending Flow Message
```c#
FlowMessageRequest flowMessageRequest = new FlowMessageRequest();
flowMessageRequest.To = sendFlowMessageViewModel.RecipientPhoneNumber;
flowMessageRequest.Interactive = new FlowMessageInteractive();

flowMessageRequest.Interactive.Header = new FlowMessageHeader();
flowMessageRequest.Interactive.Header.Type = "text";
flowMessageRequest.Interactive.Header.Text = "Header flow";

flowMessageRequest.Interactive.Body = new FlowMessageBody();
flowMessageRequest.Interactive.Body.Text = "Body flow";

flowMessageRequest.Interactive.Footer = new FlowMessageFooter();
flowMessageRequest.Interactive.Footer.Text = "Footer flow";

flowMessageRequest.Interactive.Action = new FlowMessageAction();
flowMessageRequest.Interactive.Action.Parameters = new FlowMessageParameters();
flowMessageRequest.Interactive.Action.Parameters.FlowToken = sendFlowMessageViewModel.FlowToken;
flowMessageRequest.Interactive.Action.Parameters.FlowId = sendFlowMessageViewModel.FlowId;
flowMessageRequest.Interactive.Action.Parameters.FlowCta = sendFlowMessageViewModel.FlowButtonText;
flowMessageRequest.Interactive.Action.Parameters.FlowAction = sendFlowMessageViewModel.SelectedFlowAction;
flowMessageRequest.Interactive.Action.Parameters.IsInDraftMode = (sendFlowMessageViewModel.SelectedMode.Equals("Draft", StringComparison.OrdinalIgnoreCase));

flowMessageRequest.Interactive.Action.Parameters.FlowActionPayload = new FlowActionPayload();
flowMessageRequest.Interactive.Action.Parameters.FlowActionPayload.Screen = sendFlowMessageViewModel.ScreenId;

var results = await _whatsAppBusinessClient.SendFlowMessageAsync(flowMessageRequest);
```

## Sending Flow Template Message
```c#
FlowTemplateMessageRequest flowTemplateMessageRequest = new FlowTemplateMessageRequest();
flowTemplateMessageRequest.To = sendTemplateMessageViewModel.RecipientPhoneNumber;
flowTemplateMessageRequest.Template = new();
flowTemplateMessageRequest.Template.Name = sendTemplateMessageViewModel.TemplateName;
flowTemplateMessageRequest.Template.Language = new();
flowTemplateMessageRequest.Template.Language.Code = LanguageCode.English_US;
flowTemplateMessageRequest.Template.Components = new List<FlowMessageComponent>()
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
};

var results = await _whatsAppBusinessClient.SendFlowMessageTemplateAsync(flowTemplateMessageRequest);
```

## Webhook Subscription
First, you need to setup the callback url and verify the token string for WhatsApp Cloud API to verify your callback url.
Verification part
```c#
[HttpGet("<YOUR ENDPOINT ROUTE>")]
public ActionResult<string> ConfigureWhatsAppMessageWebhook([FromQuery(Name = "hub.mode")] string hubMode,
                                                                    [FromQuery(Name = "hub.challenge")] int hubChallenge,
                                                                    [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
{
return Ok(hubChallenge);
}
```

## Receiving Messages
```c#
[HttpPost("<YOUR ENDPOINT ROUTE>")]
public IActionResult ReceiveWhatsAppTextMessage([FromBody] dynamic messageReceived)
{
// Logic to handle different type of messages received
return Ok();
}
```

## Reply to Message
```c#
TextMessageReplyRequest textMessageReplyRequest = new TextMessageReplyRequest();
textMessageReplyRequest.Context = new TextMessageContext();
textMessageReplyRequest.Context.MessageId = textMessage.SingleOrDefault().Id;
textMessageReplyRequest.To = textMessage.SingleOrDefault().From;
textMessageReplyRequest.Text = new WhatsAppText();
textMessageReplyRequest.Text.Body = "Your Message was received. Processing the request shortly";
textMessageReplyRequest.Text.PreviewUrl = false;

await _whatsAppBusinessClient.SendTextMessageAsync(textMessageReplyRequest);
```

## Verify Webhook X-Hub-Signature-256 (Credits @Tekkharibo)
```c#
[HttpPost]
public async Task<IActionResult> GetMessage()
{
    string stringifiedBody;

    string xHubSignature256 = this.HttpContext.Request.Headers["X-Hub-Signature-256"].ToString();

    using (var sr = new StreamReader(this.HttpContext.Request.Body))
    {
        stringifiedBody = await sr.ReadToEndAsync().ConfigureAwait(false);
    }

    string xHubSignature256Result = FacebookWebhookHelper.CalculateSignature(this._configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AppSecret"], stringifiedBody);

    if (!String.Equals(xHubSignature256, xHubSignature256Result, StringComparison.InvariantCultureIgnoreCase))
        return this.Unauthorized("Invalid Signature");

    return this.Ok();
}
```

## Verify Webhook X-Hub-Signature-256 (.NET 7 Minimal API) (Credits @sapharos)
```c#
app.Use((context, next) =>
{
	context.Request.EnableBuffering();
	return next();
});

// Validation implementation

string stringifiedBody;
string xHubSignature256 = context.Request.Headers["X-Hub-Signature-256"].ToString();
context.Request.Body.Seek(0, SeekOrigin.Begin);
using (var sr = new StreamReader(context.Request.Body))
{
	stringifiedBody = await sr.ReadToEndAsync().ConfigureAwait(false);
}
string xHubSignature256Result = FacebookWebhookHelper.CalculateSignature(_config.GetValue<string("Facebook:AppSecret"), stringifiedBody);
if (!String.Equals(xHubSignature256, xHubSignature256Result,StringComparison.InvariantCultureIgnoreCase))
{
	return Results.Unauthorized();
}
```

## Error handling
WhatsAppBusinessClient Throws ```WhatsappBusinessCloudAPIException``` It is your role as the developer to catch
the exception and continue processing in your application. The snippet below shows how you can catch the WhatsappBusinessCloudAPIException.

```c#
using WhatsappBusiness.CloudApi.Exceptions; // add this to your class or namespace

try
{	
	return await _whatsAppBusinessClient.SendTextMessageAsync(textMessageRequest);
}
catch (WhatsappBusinessCloudAPIException ex)
{
	_logger.LogError(ex, ex.Message);
}		
```
## Issues

If you will face any issues with the usage of this package please raise one so that we can quickly fix it as soon as possible.

## Contributing

This is an open-source project under ```MIT License``` so anyone is welcome to contribute from typos, to source code to documentation.

## Credits
1. [Gabriel](https://github.com/gabrieldwight)
2. All other contributors
