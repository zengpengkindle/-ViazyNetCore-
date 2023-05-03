using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes
{
    public class ExtensionGrantContext
    {

        public HttpContext HttpContext { get; }

        public OpenIddictRequest Request { get; }

        public ExtensionGrantContext(HttpContext httpContext, OpenIddictRequest request)
        {
            HttpContext = httpContext;
            Request = request;
        }
    }
}
