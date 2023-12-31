﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.AppApi
{
    [Area("shopmall")]
    public class BaseController : ControllerBase
    {
        public long MemberId
        {
            get
            {
                return this.HttpContext.User?.GetUserId() ?? 0;
            }
        }
    }
}
