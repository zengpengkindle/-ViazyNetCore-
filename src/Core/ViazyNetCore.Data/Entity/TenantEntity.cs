using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ViazyNetCore
{
    public interface ITenantEntity
    {
        public long TenantId { get; set; }
    }

    public class TenantEntity<TKey> : EntityUpdate<TKey>, ITenantEntity where TKey : struct
    {
        public long TenantId { get; set; }
    }

    /// <summary>
    /// 实体修改
    /// </summary>
    public class TenantEntity : TenantEntity<long>
    {
    }
}
