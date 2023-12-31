﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public class BasicTenantInfo
    {
        public int? TenantId { get; }

        /// <summary>
        /// Name of the tenant if <see cref="TenantId"/> is not null.
        /// </summary>
        public string Name { get; }

        public BasicTenantInfo(int? tenantId, string name = null)
        {
            TenantId = tenantId;
            Name = name;
        }
    }
}
