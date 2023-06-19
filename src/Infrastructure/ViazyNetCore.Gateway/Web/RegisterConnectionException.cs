using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Web
{
    public class RegisterConnectionException : Exception
    {
        public RegisterConnectionException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
