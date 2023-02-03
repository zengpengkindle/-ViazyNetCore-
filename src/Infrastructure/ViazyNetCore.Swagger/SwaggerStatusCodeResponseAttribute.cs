using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag.Annotations;

namespace ViazyNetCore
{
    public class SwaggerStatusCodeResponseAttribute : SwaggerResponseAttribute
    {
        public SwaggerStatusCodeResponseAttribute(int statusCode, string? description = null, Type? type = null) : base(statusCode, type)
        {
            this.Description = "err_code:[" + statusCode + "]" + description;
        }
    }
}
