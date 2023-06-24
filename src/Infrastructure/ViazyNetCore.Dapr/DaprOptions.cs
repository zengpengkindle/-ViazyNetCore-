using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Dapr
{
    public class DaprOptions
    {
        public string HttpEndpoint { get; set; }

        public string GrpcEndpoint { get; set; }

        public string DaprApiToken { get; set; }

        public string AppApiToken { get; set; }
    }
}
