using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Models
{
    public class SelectionFeedListDto
    {
        public string Id { get; set; }

        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌编号。
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌名称。
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目编号。
        /// </summary>
        public string CatId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目名称。
        /// </summary>
        public string CatName { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示副标题。
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示关键词。
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示最低售价。
        /// </summary>
        public decimal Price { get; set; }
    }
}
