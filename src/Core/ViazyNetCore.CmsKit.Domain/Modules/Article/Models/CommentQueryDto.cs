using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules.Models
{
    public class CommentQueryDto
    {
        public long? ArticleId { get; set; }
        public bool OnlyVisible { get; set; }
        public bool? IsNeedAudit { get; set; }
        public long UserId { get; set; }
    }
}
