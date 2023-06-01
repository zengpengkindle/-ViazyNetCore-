using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.Crm.Manage.Application.Controllers
{
    [ApiController]
    [Area("crm")]
    [Authorize]
    public class BaseController
    {

    }
}
