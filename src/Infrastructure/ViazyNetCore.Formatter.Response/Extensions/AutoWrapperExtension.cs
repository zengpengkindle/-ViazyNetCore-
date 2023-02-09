using Microsoft.AspNetCore.Builder;
using ViazyNetCore.Formatter.Response;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoWrapperExtension
    {
        public static IApplicationBuilder UseApiResponseWrapper(this IApplicationBuilder builder, Action<ResponseWrapperOptions>? options = default)
        {
            var wapperOptions = new ResponseWrapperOptions();
            if (options != null)
            {
                options.Invoke(wapperOptions);
            }
            return builder.UseMiddleware<AutoWrapperMiddleware>(wapperOptions);
        }
    }
}
