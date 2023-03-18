using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    [Table(Name = "ShopMall.ProductOuter")]
    public class ProductOuter : EntityBase<string>
    {

        /// <summary>
        /// 设置或获取一个值，表示类型名称。
        /// </summary>
        public string OuterName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型类别。
        /// </summary>
        public string OuterType { get; set; }

        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型启用时间。
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型结束时间。
        /// </summary>
        public DateTime? EndTime { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
