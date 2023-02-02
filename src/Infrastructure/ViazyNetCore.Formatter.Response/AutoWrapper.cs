using Microsoft.AspNetCore.Builder;

namespace ViazyNetCore.Formatter.Response
{
    public static class AutoWrapperExtension
    {
        public static IApplicationBuilder UseApiResponseWrapper(this IApplicationBuilder builder, ResponseWrapperOptions? options = default)
        {
            options ??= new ResponseWrapperOptions();
            return builder.UseMiddleware<AutoWrapperMiddleware>(options);
        }
    }
}
