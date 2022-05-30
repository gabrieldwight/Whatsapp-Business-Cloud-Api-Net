using System;
using System.Net;
using WhatsappBusiness.CloudApi.Response;

namespace WhatsappBusiness.CloudApi.Exceptions
{
    public class WhatsappBusinessCloudAPIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public WhatsAppErrorResponse whatsAppErrorResponse { get; set; }

        public WhatsappBusinessCloudAPIException()
        {

        }

        public WhatsappBusinessCloudAPIException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public WhatsappBusinessCloudAPIException(Exception ex, HttpStatusCode statusCode, WhatsAppErrorResponse whatsAppError) : base(ex.Message)
        {
            StatusCode = statusCode;
            whatsAppErrorResponse = whatsAppError;
        }
    }
}
