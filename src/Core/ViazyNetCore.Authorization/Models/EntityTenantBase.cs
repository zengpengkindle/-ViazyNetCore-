using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 租户接口
    /// </summary>
    public interface ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        long? TenantId { get; set; }
    }

    /// <summary>
    /// 实体租户
    /// </summary>
    public class EntityTenantBase<TKey> : EntityBase, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Description("租户Id")]
        [Column(Position = 2, CanUpdate = false)]
        [JsonProperty(Order = -20)]
        public virtual long? TenantId { get; set; }
    }

    /// <summary>
    /// 实体租户
    /// </summary>
    public class EntityTenantBase : EntityTenantBase<long>
    {
    }
}
