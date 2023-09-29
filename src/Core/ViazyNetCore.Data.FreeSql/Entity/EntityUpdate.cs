using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

namespace ViazyNetCore
{
    /// <summary>
    /// 修改接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntityUpdate<TKey>
    {
        /// <summary>
        /// 修改者Id
        /// </summary>
        long? UpdateUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        string UpdateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
    }

    public abstract class EntityUpdate<TKey> : EntityAdd<TKey>, IEntityUpdate<TKey>
    {
        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -12, CanInsert = false)]
        public virtual long? UpdateUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [Column(Position = -11, CanInsert = false), MaxLength(50)]
        public virtual string UpdateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public virtual DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 实体修改
    /// </summary>
    public abstract class EntityUpdate : EntityUpdate<long>
    {
        [Column(IsPrimary = true, Position = 1, IsIdentity = true)]
        public override long Id { get => base.Id; set => base.Id = value; }
    }

    public abstract class SnowflakeEntityUpdate : EntityUpdate<long>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Snowflake]
        [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        public virtual long Id { get; set; }
    }
}
