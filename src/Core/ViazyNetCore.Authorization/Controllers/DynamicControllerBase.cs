﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.DI;

namespace ViazyNetCore.Authorization
{
    [Authorize]
    [Area("admin")]
    [DynamicApi]
    public abstract class DynamicControllerBase : IDynamicController
    {
    }
}