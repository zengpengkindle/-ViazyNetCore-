using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

namespace ViazyNetCore
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        TKey Id { get; set; }
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Snowflake]
        [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        [JsonProperty(Order = 1)]
        public virtual TKey Id { get; set; }
    }

    public abstract class Entity : Entity<long>
    {
        [Snowflake(Enable = false)]
        [Column(Position = 1, IsIdentity = true, IsPrimary = true)]
        public override long Id { get => base.Id; set => base.Id = value; }
    }
}
