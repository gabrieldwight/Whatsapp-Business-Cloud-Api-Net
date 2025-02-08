using System.Text.Json.Serialization;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.WriteIndented = true;
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.Configure<WhatsAppBusinessCloudApiConfig>(options =>
{
    builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration").Bind(options);
});

WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
whatsAppConfig.WhatsAppBusinessPhoneNumberId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessPhoneNumberId"];
whatsAppConfig.WhatsAppBusinessAccountId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessAccountId"];
whatsAppConfig.WhatsAppBusinessId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessId"];
whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
whatsAppConfig.AppName = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AppName"];
whatsAppConfig.Version = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["Version"];

builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();