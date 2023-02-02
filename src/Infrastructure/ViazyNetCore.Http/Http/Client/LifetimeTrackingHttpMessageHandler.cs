using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
    internal class LifetimeTrackingHttpMessageHandler : DelegatingHandler
    {
        public LifetimeTrackingHttpMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
