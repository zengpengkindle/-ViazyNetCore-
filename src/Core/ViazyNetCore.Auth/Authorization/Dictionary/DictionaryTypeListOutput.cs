using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization
{
    public class DictionaryTypeListOutput
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
