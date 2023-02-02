using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag.Annotations;

namespace ViazyNetCore.Swagger
{
    public class StatusSwaggerResponseAttribute : SwaggerResponseAttribute
    {
        public StatusSwaggerResponseAttribute(int statusCode, string? description = null, Type? type = null) : base(statusCode, type)
        {
            Description = "err_code:[" + statusCode + "]" + description;
        }
    }
}
