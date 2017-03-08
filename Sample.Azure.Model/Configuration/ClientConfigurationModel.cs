using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Model.Configuration
{
    public class ClientConfigurationModel
    {
        public string UserIdentityName { get; set; }
        public bool CanViewAPISamples { get; set; }
        public string BaseUrlWebApi { get; set; }
        public string BaseUrlMVC { get; set; }
        public string AzureSearchQueryKey { get; set; }
        public string AzureSearchName { get; set; }

    }
}
