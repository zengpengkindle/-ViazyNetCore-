using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.AspNetCore.Mvc.Wrap
{
    public interface IResponseWrapHandler
    {
        Task HandleExceptionAsync(HttpContext context, Exception exception, bool logStackTrace);
    }
}
