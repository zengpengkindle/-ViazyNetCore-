using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Models
{
    public class BusinessContactListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Post { get; set; }
    }
}
