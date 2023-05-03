using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class ApplicationDescriptor: OpenIddictApplicationDescriptor
    {
        //
        // 摘要:
        //     URI to further information about client.
        public string ClientUri { get; set; }

        //
        // 摘要:
        //     URI to client logo.
        public string LogoUri { get; set; }
    }
}
