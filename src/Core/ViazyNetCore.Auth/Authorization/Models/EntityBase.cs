using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization.Model
{

    public class EntityBase : EntityBase<long>
    {
        [Column(IsPrimary = true, Position = 1, IsIdentity = true)]
        public override long Id { get => base.Id; set => base.Id = value; }
    }

    public class EntityBase<T>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(IsPrimary = true, Position = 1)]
        public virtual T Id { get; set; }
    }
}
