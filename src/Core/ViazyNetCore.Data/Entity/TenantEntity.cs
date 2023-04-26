using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ViazyNetCore
{
    public interface ITenant
    {
        public long TenantId { get; set; }
    }
}
