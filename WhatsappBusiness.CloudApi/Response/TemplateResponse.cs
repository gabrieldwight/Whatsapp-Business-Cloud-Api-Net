using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
    public class TemplateResponse : TemplateBaseResponse
    {
        public Dictionary<string, object> AdditionalFields { get; set; }
    }
}
