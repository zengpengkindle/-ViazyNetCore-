using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public class SingleSignOnException : Exception
    {
        public SingleSignOnException(): base("您的账户已在其他设备登录，若非本人操作，请及时修改密码")
        {
        }
    }
}
