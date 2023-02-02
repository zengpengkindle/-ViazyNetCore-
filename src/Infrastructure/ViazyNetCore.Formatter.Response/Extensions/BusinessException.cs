using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Formatter.Response.Extensions
{
    public class BusinessException : Exception
    {
        public BusinessException(int code, string message) : base(message)
        {

            this.Code = code;
        }

        public int Code { get; set; }

    }

}
