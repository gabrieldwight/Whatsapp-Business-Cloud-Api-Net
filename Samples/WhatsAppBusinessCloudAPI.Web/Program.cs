using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;
using WhatsAppBusinessCloudAPI.Web.Hubs;
using WhatsAppBusinessCloudAPI.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.WriteIndented = true;
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Add SignalR for real-time message display
builder.Services.AddSignalR();

builder.Services.Configure<WhatsAppBusinessCloudApiConfig>(options =>
{
    builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration").Bind(options);
});

builder.Services.Configure<EmbeddedSignupConfiguration>(options =>
{
    builder.Configuration.GetSection("EmbeddedSignupConfiguration").Bind(options);
});

WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
whatsAppConfig.WhatsAppBusinessPhoneNumberId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessPhoneNumberId"];
whatsAppConfig.WhatsAppBusinessAccountId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessAccountId"];
whatsAppConfig.WhatsAppBusinessId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessId"];
whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
whatsAppConfig.AppName = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AppName"];
whatsAppConfig.Version = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["Version"];
whatsAppConfig.WebhookVerifyToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WebhookVerifyToken"];

builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();// Removed HTTPS redirection - let IIS handle it
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map SignalR hub
app.MapHub<WhatsAppMessagesHub>("/whatsappmessageshub");

app.Run();