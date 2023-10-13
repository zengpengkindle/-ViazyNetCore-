using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit
{
    public class DictionaryTypeFindAllArgs:PaginationSort
    {
        /// <summary>
        /// 名称或
        /// </summary>
        public string? Name { get; set; }
    }
}
