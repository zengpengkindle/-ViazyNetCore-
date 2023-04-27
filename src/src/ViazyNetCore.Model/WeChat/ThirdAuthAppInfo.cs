using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    public class ThirdAuthAppInfo : EntityBase
    {
        public string UnionId { get; set; }

        public UserAccountTypes Type { get; set; }

        public string AppId { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识
        /// </summary>
        public int IsSubscribe { get; set; }

        public DateTime CreateTime { get; set; }
        public long MemberId { get; set; }
    }
}
