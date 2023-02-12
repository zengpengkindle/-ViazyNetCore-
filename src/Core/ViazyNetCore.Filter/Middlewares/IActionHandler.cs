using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.Filter.Middlewares
{
    public interface IActionHandler
    {
        Task ExecuteAsync(ActionHandlerContext context);
    }

    public class ActionHandlerContext
    {
        public HttpContext HttpContext { get; set; }
        public IActionResult Result { get; set; }
    }

}
