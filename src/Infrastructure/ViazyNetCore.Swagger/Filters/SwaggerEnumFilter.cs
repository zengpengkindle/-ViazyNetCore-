using System.ComponentModel;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ViazyNetCore.Swagger.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerEnumFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var schemaDictionaryItem in swaggerDoc.Components.Schemas)
            {
                var schema = schemaDictionaryItem.Value;
                foreach (var propertyDictionaryItem in schema.Properties)
                {
                    var property = propertyDictionaryItem.Value;
                    var propertyEnums = property.Enum;
                    if (propertyEnums != null && propertyEnums.Count > 0)
                    {
                        property.Description += DescribeEnum(propertyEnums);
                    }
                }
            }
        }

        /// <summary>
        /// 描述枚举
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        private static string DescribeEnum(IEnumerable<object> enums)
        {
            var enumDescriptions = new List<string>();
            Type? type = null;
            foreach (var enumOption in enums)
            {
                if (type == null) type = enumOption.GetType();
                enumDescriptions.Add($"{Convert.ChangeType(enumOption, type.GetEnumUnderlyingType())} = {Enum.GetName(type, enumOption)}，{GetDescription(type, enumOption)}");
            }

            return $"{Environment.NewLine}{string.Join(Environment.NewLine, enumDescriptions)}";
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetDescription(Type t, object value)
        {
            foreach (var mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (var attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
