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
	public class BulkSendWhatsAppsController : ControllerBase
    {
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
		private readonly ILogger<HomeController> _logger;		
		private readonly IWebHostEnvironment _environment;
        private readonly List<messageTypes> _msgTypes;

		public record WUpContactRecord
        {
			public string Order { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string WupNum { get; set; }
			public string MsgType { get; set; }
			public string WupMsg { get; set; }
			public string Template { get; set; }
			public string Params { get; set; }
			public string WupAtt {  get; set; }
			public string WupAttCap { get; set; }
			public string SendResult { get; set; }
		}

        private record messageTypes(string Name ,bool HasAtt );
        
        
		public BulkSendWhatsAppsController(ILogger<HomeController> logger, IWhatsAppBusinessClient whatsAppBusinessClient, IWebHostEnvironment environment)
		{
			_logger = logger;
			_whatsAppBusinessClient = whatsAppBusinessClient;			
			_environment = environment;
           
            // These are the implemente Message Types that BulkSend can handle
			_msgTypes = new List<messageTypes>();
            _msgTypes.Add(new messageTypes("Audio", true));
			_msgTypes.Add(new messageTypes("Doc - N/T", true));
			_msgTypes.Add(new messageTypes("Image - N/T", true));
            _msgTypes.Add(new messageTypes("Text - N/T", false));
			_msgTypes.Add(new messageTypes("Video - N/T", true));
		}

		private WUpContactRecord ReplaceRecordData(WUpContactRecord record)
        {   // Go through each property value, and check if any of the replacement strings can be found, and replace it

			string[] findArray = { "|FN|", "|LN|", "|Email|", "|WupNum|" };
			string[] replacementArray = {record.FirstName, record.LastName, record.Email, record.WupNum };

            object propertyValue;
            string valToCheck;

            // Loop through each property
            foreach (var property in typeof(WUpContactRecord).GetProperties())
            {

                propertyValue = property.GetValue(record);
                valToCheck = propertyValue?.ToString();
				// Skip to the next property if the value to check is null
				if (valToCheck == null)
				{
					continue;
				}

				// Iterate over each find string and corresponding replacement
				for (int i = 0; i < findArray.Length; i++)
                {
                    // Check if the find string exists in myStr
                    if (valToCheck.Contains(findArray[i]))
                    {
                        // Replace the find string with the corresponding replacement
                        valToCheck = valToCheck.Replace(findArray[i], replacementArray[i]);
                    }
                }

                // After replacement reassign the value back to the property
                property.SetValue(record, valToCheck);

            }

            return record;

		}

        /// <summary>
        /// 1. Read the CSV file and go through each record
        /// 2. Prep the record data
        /// 3. Build the sendWhatsAppPayload so it can be used to send the WhatsApp
        /// 4. Send the WhatsApp for this record, get the WAMID and store it in the SendResult
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ReadAndTraverseCSV(FileInfo fileInfo)
        {
            // Read the CSV File into a DT
            DataTable dataTable = ReadCSVFile(fileInfo);

            // Use the SendMessageController
            SendMessageController sendMessageController = new(_logger, _whatsAppBusinessClient, _environment);
            FileManagmentController fileManagementController = new(_logger, _whatsAppBusinessClient, _environment );
            SendTemplate_media_ParameterPayload sendWhatsAppPayload = new();
            FileInfo uploadMediaPayload = new();            

            // We will build a unique list of Attachments so we can upload the Attachments to WhatsApp and use the IDs
            Dictionary<string, string> uniqueAttWithMediaID = new Dictionary<string, string>();

            WUpContactRecord wUpContact = new WUpContactRecord();

            foreach (DataRow row in dataTable.Rows)
            {
                // First test Key elements, if any error continue to next record
                wUpContact.WupNum = sendMessageController.PrepNumber(row["WUp Num"].ToString());    // Prep the number and return the prepped number
                if (wUpContact.WupNum == "-1")
				{   // Error on the number, bounce to next record
					row["SendResult"] = "ERROR: Not a WhatsApp number?";
					continue; 
                }

				wUpContact.MsgType = row["MsgType"].ToString();
				//if (!_msgTypes.Contains(wUpContact.MsgType))   
                if (_msgTypes.Any(msgType => msgType.Name != wUpContact.MsgType))    
				{   // MsgType is not in the MsgTypeImplemented list, bounce to next record
					row["SendResult"] = "ERROR: Message Type not implemented";
					continue;
				}
                
                // Get rest of the Data for this record
                // string order = row["Order"].ToString();
                wUpContact.FirstName = row["First Name"].ToString();
				wUpContact.LastName = row["Last Name"].ToString();
                wUpContact.Email = row["Email"].ToString();
                wUpContact.WupMsg = row["WUp Msg"].ToString();
                wUpContact.Template = row["Template"].ToString();
                wUpContact.Params = row["Params"].ToString();
				wUpContact.WupAtt = row["Att"].ToString();
				wUpContact.WupAttCap = row["WUp Att Cap"].ToString();
				wUpContact.SendResult = row["SendResult"].ToString();

                // Search and Replace any valid Placeholders
                wUpContact = ReplaceRecordData(wUpContact);

                // For the following Message Types there MUST be an attachement
                if (_msgTypes.Any(msgType => msgType.Name == wUpContact.MsgType && msgType.HasAtt))
                { 

CJM - Move all instance creation to outside of the loop
                    sendWhatsAppPayload.Media = new WhatsAppMedia();
				    sendWhatsAppPayload.Media.Caption = wUpContact.WupAttCap;

				    // Build the unique list of attachments, if a new attachment is found, upload it and get the ID
				    if (uniqueAttWithMediaID.ContainsKey(wUpContact.WupAtt))
                    {
                        sendWhatsAppPayload.Media.ID = uniqueAttWithMediaID[wUpContact.WupAtt]; // Retrieve the corresponding ID                                                                             
                    }
                    else
                    {
                        // wUpAtt is not in the Dictionary, so we will upload the Media to Whatsapp and get the Media ID for use
                        uploadMediaPayload.fileUploadMethod = "Normal";
                        uploadMediaPayload.filePath = Path.Combine(_environment.WebRootPath, fileManagementController._localServerPaths.LocalFileUploadPath);
                        uploadMediaPayload.fileName = wUpContact.WupAtt;

                        FileInfo fileUploadedInfo = await fileManagementController.UploadFileToWhatsApp(uploadMediaPayload);                    

                        if (uploadMediaPayload.fileUploadSuccess)
                        {   //File Uploaded to WhatsApp successfully                     
                            sendWhatsAppPayload.Media.ID = uploadMediaPayload.fileWhatsAppID;
                        }
                        else
                        {   // The file was not uploaded to WhatsApp: This message cannot be sent, Skip to next record..
						    row["SendResult"] = "ERROR: File could not be Uploaded to WhatsApp";
						    continue;
					    }
                        uniqueAttWithMediaID.Add(wUpContact.WupAtt, sendWhatsAppPayload.Media.ID); // Add the entry to the Dictionary if not found
                    }
                }
                

				
				// Prep to send the WhatsApp
				sendWhatsAppPayload.SendText = new SendTextPayload()
                {
                    ToNum = wUpContact.WupNum
                };

				// Split die Params into a List
				string strParams = wUpContact.Params;
				List<string> listParams = strParams.Split(new string[] { "#" }, StringSplitOptions.None).ToList();

                sendWhatsAppPayload.Template = new WhatsappTemplate()
                {
                    Name = wUpContact.Template,
                    Params = listParams
                };



				string WAMIds = sendMessageController.GetWAMId((await sendMessageController.SendTemplate_video_ParameterAsync(sendWhatsAppPayload)).Value);				

                row["SendResult"] = WAMIds;

            }
            
            WriteDataTableToCSV(dataTable, fileInfo);

            return Ok("All good");
        }

        private static DataTable ReadCSVFile(FileInfo fileInfo)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var reader = new StreamReader(fileInfo.filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();

                    // Add columns to the DataTable
                    foreach (string header in csv.HeaderRecord)
                    {
                        dataTable.Columns.Add(header.Trim());
                    }

                    // Read data and add rows to the DataTable
                    while (csv.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            row[col.ColumnName] = csv.GetField(col.DataType, col.ColumnName);
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, such as logging or throwing
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                throw; // Rethrow the exception or handle it as required
            }

            return dataTable;
        }

		private static void WriteDataTableToCSV(DataTable dataTable, FileInfo fileInfo)
		{
			StreamWriter sw = new StreamWriter(fileInfo.filePath, false);
			//headers
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				sw.Write(dataTable.Columns[i]);
				if (i < dataTable.Columns.Count - 1)
				{
					sw.Write(",");
				}
			}
			sw.Write(sw.NewLine);
			foreach (DataRow dr in dataTable.Rows)
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					if (!Convert.IsDBNull(dr[i]))
					{
						string value = dr[i].ToString();
						if (value.Contains(','))
						{
							value = System.String.Format("\"{0}\"", value);
							sw.Write(value);
						}
						else
						{
							sw.Write(dr[i].ToString());
						}
					}
					if (i < dataTable.Columns.Count - 1)
					{
						sw.Write(",");
					}
				}
				sw.Write(sw.NewLine);
			}
			sw.Close();
		}

	}

}
