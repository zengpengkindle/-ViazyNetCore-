using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.DynamicControllers;

namespace ViazyNetCore.Authorization
{
    [Authorize]
    [Area("admin")]
    [DynamicApi]
    public abstract class DynamicControllerBase : IDynamicController
    {
    }
}
