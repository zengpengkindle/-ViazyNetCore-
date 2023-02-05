using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiVersioningConfurationExtensions
    {
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("version_code");
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ErrorResponses = new ApiVersioningErrorResponseProvider();

            }).AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VV";
                option.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }

    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var response = new ObjectResult("Request invalid. Unsupported api version.");
            response.StatusCode = (int)HttpStatusCode.NotFound;

            return response;
        }
    }
}
