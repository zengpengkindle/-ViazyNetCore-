using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace ViazyNetCore
{
    public abstract class EntityMember<TKey> : Entity<TKey>, IMember, IDelete
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        [Description("会员Id")]
        [Column(Position = -23, CanUpdate = false)]
        public virtual long? MemberId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -21, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -22)]
        public virtual bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// 实体会员
    /// </summary>
    public abstract class EntityMember : EntityMember<long>
    {
        [Column(IsPrimary = true, Position = 1, IsIdentity = true)]
        [Snowflake(Enable = false)]
        public override long Id { get => base.Id; set => base.Id = value; }
    }

    public abstract class SnowflakeEntityMember : EntityMember<long>
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
