﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using FreeSql.DataAnnotations;
using ViazyNetCore.Data.FreeSql;

namespace ViazyNetCore
{
    /// <summary>
    /// 添加接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntityAdd<TKey> : IHasCreateTime
    {
        /// <summary>
        /// 创建者用户Id
        /// </summary>
        long? CreateUserId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }
    }

    public abstract class EntityAdd<TKey> : Entity<TKey>, IEntityAdd<TKey>
    {
        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        [Column(Position = -22, CanUpdate = false)]
        public virtual long? CreateUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [Column(Position = -21, CanUpdate = false), MaxLength(50)]
        public virtual string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public virtual DateTime? CreateTime { get; set; }
    }

    /// <summary>
    /// 实体创建
    /// </summary>
    public abstract class EntityAdd : EntityAdd<long>
    {
        [Column(IsPrimary = true, Position = 1, IsIdentity = true)]
        [Snowflake(Enable = false)]
        public override long Id { get => base.Id; set => base.Id = value; }
    }

    public abstract class SnowflakeEntityAdd : EntityAdd<long>
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
