namespace WhatsAppBusinessCloudAPI.Web.ViewModel
{
    public class ExchangeTokenRequestViewModel
    {
        public string? Code { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? RedirectUri { get; set; }
    }
}