using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ViazyNetCore.Swagger
{
    public class DynamicDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var orderTagList = new ConcurrentDictionary<string, int>();
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var order = 0;
                var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                var objOrderAttribute = actionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
                if (objOrderAttribute != null)
                {
                    var orderAttribute = objOrderAttribute as DynamicApiAttribute;
                    order = 0;
                }
                orderTagList.TryAdd(actionDescriptor.ControllerName, order);
            }

            swaggerDoc.Tags = swaggerDoc.Tags
                                        .OrderBy(u => orderTagList.TryGetValue(u.Name, out int order) ? order : 0)
                                        .ThenBy(u => u.Name)
                                        .ToArray();
        }
    }
}
