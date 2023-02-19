using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.DI;

namespace ViazyNetCore.Auth.Authorization.Controllers
{
    [Authorize]
    [ApiController]
    [DynamicApi()]
    public abstract class DynamicControllerBase : IDynamicController
    {
    }
}
