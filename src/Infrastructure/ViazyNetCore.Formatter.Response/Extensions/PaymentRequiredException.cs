using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Formatter.Response.Extensions
{
    /// <summary>
    /// 权限越界异常
    /// </summary>
    public class PaymentRequiredException : Exception
    {
        public PaymentRequiredException() : base("当前为体验版，升级为旗舰版，享受更多权益") 
        {

        }
    }
}
