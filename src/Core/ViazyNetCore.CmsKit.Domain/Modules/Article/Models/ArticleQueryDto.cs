using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules.Models
{
    public class ArticleQueryDto
    {
        /// <summary>
        /// 仅请求已发布文章
        /// </summary>
        public bool OnlyPublished { get; set; } = false;

        /// <summary>
        /// 文章状态
        /// </summary>
        public ComStatus? Status { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; } = 0;
        public string? Search { get; set; }
        public ArticleType? Type { get; set; }
    }
}
