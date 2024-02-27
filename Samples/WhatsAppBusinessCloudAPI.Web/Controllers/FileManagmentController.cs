using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using System.Globalization;
using CsvHelper;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsAppBusinessCloudAPI.Web.Extensions.Alerts;
using System.IO;
using WhatsappBusiness.CloudApi.Media.Requests;
using WhatsAppBusinessCloudAPI.Web.ViewModel;
using WhatsappBusiness.CloudApi.Exceptions;
using Newtonsoft.Json;

namespace WhatsAppBusinessCloudAPI.Web.Controllers
{
	public record FileInfo
	{
		public string fileName { get; set; }
        public string filePath { get; set; }
		public string fileContentType { get; set; }
		public long fileSize { get; set; }
		public string fileUploadMethod { get; set; } = "Normal";
        public bool fileUploadSuccess { get; set; }
		public string fileURL{ get; set;}
		public string fileWhatsAppID { get; set;}
		public ResumableFileInfo fileResumableInfo { get; set; }
	}

	public record ResumableFileInfo
	{
		public string H {  get; set; }
		public string StatusID { get; set; }
		public long FileOffset { get; set; }
	}

	public record LocalFileUpDownLoadPath
	{
		public string LocalFileUploadPath { get; set; } = "Application_Files\\MediaUploads\\";
		public string LocalFileDownloadPath { get; set; } = "Application_Files\\MediaDownloads\\";
	}

	public class FileManagmentController : ControllerBase
    {
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
		private readonly ILogger<HomeController> _logger;		
		private readonly IWebHostEnvironment _environment;
		public readonly LocalFileUpDownLoadPath _localServerPaths = new();

		public FileManagmentController(ILogger<HomeController> logger, IWhatsAppBusinessClient whatsAppBusinessClient, IWebHostEnvironment environment)
		{
			_logger = logger;
			_whatsAppBusinessClient = whatsAppBusinessClient;			
			_environment = environment;			

			// Build configuration
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Access values
			_localServerPaths.LocalFileUploadPath = configuration["AppInfo:LocalServerUploadPath"] ?? "Application_Files\\MediaUploads\\";
			_localServerPaths.LocalFileDownloadPath = configuration["AppInfo:LocalServerDownloadPath"] ?? "Application_Files\\MediaDownload\\";


		}
       
        [HttpPost]
        [Route("[action]")]		
		public async Task<FileInfo> UploadFileToLocalServer([FromForm] IFormFile file)
        {
			// This is to upload the fiole to the Local server

            FileInfo fileInfo = new();
            try
            {
				fileInfo.fileSize = file.Length;				

				// combining GUID to create unique name before saving in wwwroot
				fileInfo.fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // getting file type
                fileInfo.fileContentType = file.ContentType;

				var rootPath = Path.Combine(_environment.WebRootPath, _localServerPaths.LocalFileUploadPath);

				if (!Directory.Exists(rootPath))
				{ // Create the Dir if it does not exist
					Directory.CreateDirectory(rootPath);
				}

				// getting full path inside wwwroot/images
				fileInfo.filePath = Path.Combine(rootPath, fileInfo.fileName);

				// copying file
				using (var stream = new FileStream(fileInfo.filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

                fileInfo.fileUploadSuccess = true ;

                return fileInfo;
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, ex.Message);
				
				fileInfo.fileUploadSuccess= false ;
                return fileInfo;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public string GetFileType(string fileName = "IMG_2883.jpg")
        {
            try
            {
                const string DefaultContentType = "application/octet-stream";
                // getting full path inside wwwroot/images
                string filePath = Path.Combine(_environment.WebRootPath, _localServerPaths.LocalFileUploadPath, fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (System.IO.File.Exists(filePath))
                {
                    if (!provider.TryGetContentType(filePath, out string contentType))
                    {
                        contentType = DefaultContentType;
                    }
                    return contentType;
                }
                else
                {
                    throw new Exception($"'{fileName}' does not exist in the '{_localServerPaths.LocalFileUploadPath}' directory");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
			}
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<FileInfo> UploadFileToWhatsApp(FileInfo fileInfo)
		{
			try
			{
				if (fileInfo.fileUploadMethod.ToUpper() == "NORMAL")
				{
					//Normal Upload to WhatsApp
					UploadMediaRequest uploadMediaRequest = new UploadMediaRequest();

					// Compile the Full path and File name
					string fullpath = fileInfo.filePath ?? Path.Combine(_environment.WebRootPath, _localServerPaths.LocalFileUploadPath);
					// Check if the fullpath already contains the fileName
					if (!fullpath.Contains(fileInfo.fileName))
					{
						// Combine the fullpath and fileName only if the fileName is not already in the fullpath
						uploadMediaRequest.File = Path.Combine(fullpath, fileInfo.fileName);
					}
					else
					{
						// Set the file path directly without combining, as the fileName is already part of the fullpath
						uploadMediaRequest.File = fullpath;
					}

					// If we do not know the File content type then get it
					uploadMediaRequest.Type = fileInfo.fileContentType ?? GetFileType(uploadMediaRequest.File);

					var uploadMediaResult = await _whatsAppBusinessClient.UploadMediaAsync(uploadMediaRequest);
					fileInfo.fileWhatsAppID = uploadMediaResult.MediaId;
					fileInfo.fileUploadSuccess = true;
				}
				else // Resumable upload generates header file response to be used for creating message templates
				{
					// ********* Resumable Upload is not for WhatsApp Purposes, It does not return a WhatsApp Media ID *********
					var resumableUploadMediaResult = await _whatsAppBusinessClient.CreateResumableUploadSessionAsync(fileInfo.fileSize, fileInfo.fileContentType, fileInfo.fileName);

					if (resumableUploadMediaResult is not null)
					{
						var uploadSessionId = resumableUploadMediaResult.Id;
						var resumableUploadResponse = await _whatsAppBusinessClient.UploadFileDataAsync(uploadSessionId, fileInfo.filePath, fileInfo.fileContentType);
						var queryResumableUploadStatus = await _whatsAppBusinessClient.QueryFileUploadStatusAsync(uploadSessionId);

						// Create the Resumable info in fileInfo
						fileInfo.fileResumableInfo = new ResumableFileInfo();

						if (resumableUploadResponse is not null)
						{
							fileInfo.fileResumableInfo.H = resumableUploadResponse.H;
						}

						if (queryResumableUploadStatus is not null)
						{
							fileInfo.fileResumableInfo.StatusID = queryResumableUploadStatus.Id;
							fileInfo.fileResumableInfo.FileOffset = queryResumableUploadStatus.FileOffset;
						}
						fileInfo.fileUploadSuccess = true;
					}
				}
				return fileInfo;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				fileInfo.fileUploadSuccess = false;
				return fileInfo;
			}


		}
	}

}
