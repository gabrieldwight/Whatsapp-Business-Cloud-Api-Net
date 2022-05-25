using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappBusiness.CloudApi.Exceptions
{
    public class WhatsappBusinessCloudAPIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public WhatsappBusinessCloudAPIException()
        {

        }

        public WhatsappBusinessCloudAPIException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
