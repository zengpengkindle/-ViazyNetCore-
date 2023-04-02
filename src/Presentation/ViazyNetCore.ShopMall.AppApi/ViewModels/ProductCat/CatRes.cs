using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels
{
    public class CatRes
    {
        public string Id { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示上级编号。
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否父级。
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示排序。
        /// </summary>
        public int Sort { get; set; }
    }
}
