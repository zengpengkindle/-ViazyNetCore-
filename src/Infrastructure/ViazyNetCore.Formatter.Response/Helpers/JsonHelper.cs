using Newtonsoft.Json;

namespace ViazyNetCore.Formatter.Response.Helpers
{
    public static class JSONHelper
    {
        public static JsonSerializerSettings GetJSONSettings()
        {
            return new CamelCaseContractResolverJsonSettings().GetJSONSettings();
        }
    }
}
