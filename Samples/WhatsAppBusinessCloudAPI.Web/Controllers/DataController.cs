using Microsoft.AspNetCore.Mvc;
using WhatsappBusiness.CloudApi.Interfaces;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
	public record SQLite
	{
		public string Path { get; set; } = "Application_Files\\DataBase\\";
		public string DBName { get; set; } = "AppDB.db";
	}

	public class DataController : Controller
	{		
		private readonly ILogger<HomeController> _logger;
		private readonly IWebHostEnvironment _environment;
		public readonly SQLite _SQLite = new();

		public DataController(ILogger<HomeController> logger, IWebHostEnvironment environment)
		{
			_logger = logger;			
			_environment = environment;

			// Build configuration
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Access values
			_SQLite.Path = configuration["AppInfo:SQLitePath"] ?? "Application_Files\\MediaUploads\\";
			_SQLite.DBName = configuration["AppInfo:SQLiteDBName"] ?? "AppDB.db";


		}

		//public string CreateDB()
		//{

		//}
	}
}
