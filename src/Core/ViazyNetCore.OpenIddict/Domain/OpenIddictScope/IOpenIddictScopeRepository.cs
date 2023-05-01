﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.OpenIddict.Domain.Entity;

namespace ViazyNetCore.OpenIddict.Domain
{
    public interface IOpenIddictScopeRepository : IBaseRepository<OpenIddictScope, long>
    {

    }
}
