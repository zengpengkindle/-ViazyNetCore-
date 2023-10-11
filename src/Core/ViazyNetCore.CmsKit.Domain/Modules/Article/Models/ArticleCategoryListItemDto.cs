using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules.Models
{
    public class ArticleCategoryListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public ComStatus Status { get; set; } = ComStatus.Enabled;
        public DateTime CreateTime { get; set; }
    }
}
