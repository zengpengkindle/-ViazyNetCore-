using System;
using System.Collections.Generic;
using System.Text;
using FreeSql;

namespace ViazyNetCore.Model
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
