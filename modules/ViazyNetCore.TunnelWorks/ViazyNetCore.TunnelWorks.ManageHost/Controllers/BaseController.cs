using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.TunnelWorks.ManageHost.Controllers
{
    [ApiController]
    [Authorize]
    [Area("tunnelwork")]
    public class BaseController : ControllerBase
    {

    }
}
