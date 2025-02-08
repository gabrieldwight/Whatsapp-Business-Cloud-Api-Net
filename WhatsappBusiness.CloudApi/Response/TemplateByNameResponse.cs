using System.Collections.Generic;

namespace WhatsappBusiness.CloudApi.Response
{
	public class TemplateByNameResponse : TemplateBaseResponse
	{
        public Dictionary<string, object> AdditionalFields { get; set; }
    }
}
