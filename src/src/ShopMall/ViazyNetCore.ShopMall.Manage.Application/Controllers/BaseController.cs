using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers
{
    [ApiController]
    [Area("shopmall")]
    [Authorize]
    public class BaseController
    {

    }
}
