using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ViazyNetCore.Swagger.Filters
{
    /// <summary>
    /// Swagger Enum 枚举类型返回描述
    /// </summary>
    public class SwaggerEnumFilter : IDocumentFilter
    {
        /// <inheritdoc/>
        public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            Dictionary<string, Type> dict = GetAllEnum();
            foreach (var propertyDictionaryItem in swaggerDoc.Components.Schemas)
            {
                var typeName = propertyDictionaryItem.Key;
                var property = propertyDictionaryItem.Value;
                var propertyEnums = property.Enum;
                Type? itemType = null;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    if (dict.ContainsKey(typeName))
                    {
                        itemType = dict[typeName];
                    }
                    else
                    {
                        itemType = null;
                    }
                    List<OpenApiInteger> list = new List<OpenApiInteger>();

                    foreach (var val in property.Enum)
                    {
                        list.Add((OpenApiInteger)val);
                    }
                    propertyEnums.Clear();

                    DescribeEnum(itemType, list).ToList().ForEach(item =>
                    {
                        property.Enum.Add(item);
                    });
                }
            }
        }

        private static Dictionary<string, Type> GetAllEnum()
        {
            var assemblies = RuntimeHelper.GetAllAssemblies();

            Dictionary<string, Type> dict = new Dictionary<string, Type>();
            foreach (var ass in assemblies)
            {
                Type[] types = ass.GetTypes();
                foreach (Type item in types)
                {
                    if (item.IsEnum)
                    {
                        dict.Add(item.Name, item);
                    }
                }
            }
            return dict;
        }

        private static IEnumerable<IOpenApiAny> DescribeEnum(Type? type, List<OpenApiInteger> enums)
        {
            if (type == null)
            {
                foreach (var item in enums)
                {
                    yield return item;
                }
                yield break;
            }

            foreach (var enumOption in enums)
            {
                var value = Enum.Parse(type, enumOption.Value.ToString());
                var desc = GetDescription(type, value);

                if (string.IsNullOrEmpty(desc))
                    desc = $"{enumOption.Value},/** {Enum.GetName(type, value)} */;";
                else
                    desc = $"{enumOption.Value},/** {Enum.GetName(type, value)}_{desc} */";

                yield return new OpenApiString(desc);
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
