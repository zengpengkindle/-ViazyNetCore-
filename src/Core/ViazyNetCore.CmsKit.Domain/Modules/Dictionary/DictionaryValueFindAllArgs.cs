using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit
{
    public class DictionaryValueFindAllArgs : PaginationSort
    {
        /// <summary>
        /// 名称或编码
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required]
        public long DictionaryTypeId { get; set; }
    }
}
